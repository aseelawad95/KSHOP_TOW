using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class NotificationRepository : Genericrepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
