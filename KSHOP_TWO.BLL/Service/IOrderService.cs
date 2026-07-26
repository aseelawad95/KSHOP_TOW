using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface IOrderService
    {
         Task<List<OrderResponse>> GetOrders(string UserId);

        Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId);

        Task<bool> CancelOrder(string userId, int orderId);

        //for admin

         Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status);

        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request);

    }
}
