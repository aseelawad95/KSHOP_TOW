using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Microsoft.Extensions.Logging;

namespace KSHOP_TWO.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            INotificationService notificationService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<List<OrderResponse>> GetOrders(string UserId)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o => o.UserId == UserId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                }
            );
            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId)
        {
            var order = await _orderRepository.GetOne(
                filter: o => o.UserId == userId && o.Id == orderId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"
                }
            );
            if (order == null) return null;

            return order.Adapt<OrderDetailsResponse>();
        }

        public async Task<bool> CancelOrder(string userId, int orderId)
        {
            var order = await _orderRepository.GetOne(
                filter: o => o.UserId == userId && o.Id == orderId);

            if (order == null) return false;
            if (order.OrderStatus == OrderStatusEnum.Pending) return false;

            order.OrderStatus = OrderStatusEnum.Cancelled;

            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status)
        {
            var orders = await _orderRepository.GetAllAsync(
                filter: o => o.OrderStatus == status
            );
            return orders.Adapt<List<OrderResponse>>();
        }

        public async Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request)
        {
            var order = await _orderRepository.GetOne(o => o.Id == orderId);

            if (order == null) return false;
            if (order.OrderStatus == OrderStatusEnum.Cancelled || order.OrderStatus == OrderStatusEnum.Delivered) return false;
            if ((int)request.Status != (int)order.OrderStatus + 1) return false;

            order.OrderStatus = request.Status;

            var updated = await _orderRepository.UpdateAsync(order);
            if (!updated) return false;

            await NotifyOrderStatusAsync(order);

            return true;
        }

        private async Task NotifyOrderStatusAsync(Order order)
        {
            var notification = BuildOrderNotification(order);
            if (notification is null) return;

            try
            {
                await _notificationService.SendNotificationAsync(
                    order.UserId,
                    notification.Value.Title,
                    notification.Value.Message,
                    notification.Value.Type);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification for order {OrderId}", order.Id);
            }
        }

        private static (NotificationType Type, string Title, string Message)? BuildOrderNotification(Order order)
        {
            return order.OrderStatus switch
            {
                OrderStatusEnum.Approved => (NotificationType.OrderApproved, "Order Approved", $"Your order #{order.Id} has been approved."),
                OrderStatusEnum.Shipped => (NotificationType.OrderShipped, "Order Shipped", $"Your order #{order.Id} is on its way."),
                OrderStatusEnum.Delivered => (NotificationType.OrderDelivered, "Order Delivered", $"Your order #{order.Id} has been delivered."),
                OrderStatusEnum.Cancelled => (NotificationType.OrderCancelled, "Order Cancelled", $"Your order #{order.Id} was cancelled."),
                _ => null
            };
        }
    }
}