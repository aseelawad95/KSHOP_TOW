using KSHOP_TWO.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveNotification(NotificationResponse notification);
    }
}
