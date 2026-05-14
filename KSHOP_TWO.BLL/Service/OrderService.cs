using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderResponse>> GetUserOrders(string UserId)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter : o => o.UserId == UserId,
                includes : new[]
                {
                    nameof (Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"

                }

                );
            return orders.Adapt<List<OrderResponse>>();
        }
    }
}
