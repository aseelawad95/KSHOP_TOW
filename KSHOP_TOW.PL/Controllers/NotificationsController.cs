using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
            => Ok(await _notificationService.GetUserNotificationsAsync(UserId));

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
            => Ok(new { count = await _notificationService.GetUnreadCountAsync(UserId) });

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
            => await _notificationService.MarkAsReadAsync(id, UserId) ? NoContent() : NotFound();

        [HttpPost("device-token")]
        public async Task<IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenRequest request)
        {
            await _notificationService.RegisterDeviceTokenAsync(UserId, request.Token);
            return NoContent();
        }
    }
}

