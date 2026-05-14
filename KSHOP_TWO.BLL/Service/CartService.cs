using KSHOP_TWO.DAL.DTO.Request;
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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> AddToCart(AddToCartRequest request, string UserId)
        {
            var product = await _productRepository.GetOne(p => p.Id == request.ProductId);
            if (product == null) return false;

                var existingItem = await _cartRepository.GetOne(c => c.ProductId == request.ProductId && c.UserId == UserId);

            var currentCount = existingItem?.Count ?? 0;
            var newCount = currentCount + request.Count;
            if (newCount > product.Quantity)
            {
                return false; 
            }

            if (existingItem != null)
            {
                existingItem.Count += newCount;
                await _cartRepository.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = request.Adapt<Cart>();
               cartItem.UserId = UserId;
                await _cartRepository.CreateAsync(cartItem);
            }
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
           var items =  _cartRepository.GetAllAsync(c => c.UserId == userId).Result;
            if (!items.Any()) return false;
            await _cartRepository.DeleteRangeAsync(items);
            return true;

        }

        public async Task<List<CartResponse>> GetCart(string userId)
        {
            var items = await _cartRepository.GetAllAsync(
                filter : c=>c.UserId == userId,
                includes : new string[]
                {
                    nameof(Cart.Product),
                    $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                }
                );
            return items.Adapt<List<CartResponse>>();
        }

        public async Task<bool> RemoveItem(int productId, string userId)
        {
            var item = await _cartRepository.GetOne(c => c.ProductId == productId && c.UserId == userId);
            if (item == null) return false;
            await _cartRepository.DeleteAsync(item);
            return true;
        }

        public async Task<bool> UpdateQuantity(int productId, int count, string userId)
        {
            var item = await _cartRepository.GetOne(c => c.ProductId == productId && c.UserId == userId);
            if (item == null) return false;

            var product = await _productRepository.GetOne(p => p.Id == productId);
            if (count > product.Quantity) return false;

            item.Count = count;
            return await _cartRepository.UpdateAsync(item);
        }
    }
}
