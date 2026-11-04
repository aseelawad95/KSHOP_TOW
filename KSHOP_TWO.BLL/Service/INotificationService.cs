using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface INotificationService
    {
        Task<int> GetUnreadCountAsync(string userId);
        Task<IEnumerable<NotificationResponse>> GetUserNotificationsAsync(string userId);
        Task<bool> MarkAsReadAsync(int id, string userId);
        Task SendNotificationAsync(string userId, string title, string message, NotificationType type);
        Task RegisterDeviceTokenAsync(string userId, string token);
    }
}
