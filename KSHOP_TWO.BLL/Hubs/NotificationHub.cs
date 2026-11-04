using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Hubs
{
    [Authorize]
    public class NotificationHub : Hub<INotificationClient>
    {
        public const string AdminsGroup = "Admins";
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Hub connected: user={UserId}, conn={ConnId}",
                Context.UserIdentifier, Context.ConnectionId);

            if (Context.User?.IsInRole("Admin") == true)
                await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);

            await base.OnConnectedAsync();
        }
    }
}
