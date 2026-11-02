using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Models
{
    public enum NotificationType
    {
        OrderPlaced,
        OrderApproved,
        OrderRejected,
        OrderShipped,
        OrderDelivered,
        OrderCancelled,
        NewMessage
    }
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
