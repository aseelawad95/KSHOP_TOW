using FirebaseAdmin.Messaging;
using KSHOP_TWO.BLL.Hubs;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceTokenRepository _deviceTokenRepository;
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IDeviceTokenRepository deviceTokenRepository,
            IHubContext<NotificationHub, INotificationClient> hubContext,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _deviceTokenRepository = deviceTokenRepository;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _notificationRepository
                .GetQuerable(n => n.UserId == userId && !n.IsRead)
                .CountAsync();
        }

        public async Task<IEnumerable<NotificationResponse>> GetUserNotificationsAsync(string userId)
        {
            var notifications = await _notificationRepository
                .GetQuerable(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications.Adapt<IEnumerable<NotificationResponse>>();
        }

        public async Task<bool> MarkAsReadAsync(int id, string userId)
        {
            var notification = await _notificationRepository
                .GetOne(n => n.Id == id && n.UserId == userId);

            if (notification is null) return false;

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        public async Task RegisterDeviceTokenAsync(string userId, string token)
        {
            var existing = await _deviceTokenRepository.GetOne(t => t.Token == token);

            if (existing is null)
            {
                await _deviceTokenRepository.CreateAsync(new DeviceToken
                {
                    UserId = userId,
                    Token = token
                });
                return;
            }

            if (existing.UserId != userId)
            {
                existing.UserId = userId;
                await _deviceTokenRepository.UpdateAsync(existing);
            }
        }

        public async Task SendNotificationAsync(string userId, string title, string message, NotificationType type)
        {
            var notification = new KSHOP_TWO.DAL.Models.Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                IsRead = false
            };

            await _notificationRepository.CreateAsync(notification);

            var dto = notification.Adapt<NotificationResponse>();

            await SendRealTimeAsync(userId, dto);
            await SendPushAsync(userId, title, message);
        }

        private async Task SendRealTimeAsync(string userId, NotificationResponse dto)
        {
            try
            {
                _logger.LogInformation("SignalR sending to user {UserId}, notification {Id}", userId, dto.Id);
                await _hubContext.Clients.User(userId).ReceiveNotification(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SignalR send failed for user {UserId}", userId);
            }
        }

        private async Task SendPushAsync(string userId, string title, string body)
        {
            try
            {
                var tokens = await _deviceTokenRepository
                    .GetQuerable(t => t.UserId == userId)
                    .Select(t => t.Token)
                    .ToListAsync();

                if (tokens.Count == 0) return;

                var multicast = new MulticastMessage
                {
                    Tokens = tokens,
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance
                    .SendEachForMulticastAsync(multicast);

                if (response.FailureCount == 0) return;

                for (int i = 0; i < response.Responses.Count; i++)
                {
                    var result = response.Responses[i];
                    if (result.IsSuccess) continue;

                    var errorCode = result.Exception?.MessagingErrorCode;

                    if (errorCode == MessagingErrorCode.Unregistered ||
                        errorCode == MessagingErrorCode.InvalidArgument)
                    {
                        var deadToken = await _deviceTokenRepository
                            .GetOne(t => t.Token == tokens[i]);

                        if (deadToken is not null)
                            await _deviceTokenRepository.DeleteAsync(deadToken);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "FCM transient failure for user {UserId}: {Error}",
                            userId, result.Exception?.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FCM send failed for user {UserId}", userId);
            }
        }
    }
}
