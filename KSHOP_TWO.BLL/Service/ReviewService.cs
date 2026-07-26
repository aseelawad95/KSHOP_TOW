using KSHOP_TWO.DAL.DTO.Request;
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
    public class ReviewService : IRewiewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository orderRepository,IReviewRepository reviewRepository) {
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task<bool> AddReview(string UserId, AddReviewRequest requset)
        {
            var purchasedOrder = await _orderRepository.GetOne(
                filter: o => o.UserId == UserId &&
                    o.OrderStatus == OrderStatusEnum.Delivered &&
                    o.OrderItems.Any(oi => oi.ProductId == requset.ProductId),

                includes: new[]
                {
                    nameof(Order.OrderItems)
                }
            );
            if (purchasedOrder == null) return false;
            var AlredyReviews = await _reviewRepository.GetOne(
                o => o.UserId == UserId&&
                o.ProductId == requset.ProductId
                );

            if (AlredyReviews != null) return false;
            var review = requset.Adapt<Review>();
            review.UserId = UserId;

            await _reviewRepository.CreateAsync(review);

            return true;
            {
                
            }
        }
    }
}
