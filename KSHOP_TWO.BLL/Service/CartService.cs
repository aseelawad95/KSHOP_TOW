using Azure;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = KSHOP_TWO.DAL.Models.Product;

namespace KSHOP_TWO.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository,
            IFileService fileService
            )
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _fileService = fileService;
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
            var response = items.Adapt<List<CartResponse>>();

            for (int i = 0; i < items.Count; i++)
            {
                var product = items[i].Product;
                if (product != null && !string.IsNullOrEmpty(product.MainImage))
                {
                    response[i].ProductImage = _fileService.GetImageUrl(product.MainImage);
                }
            }

            return response;
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
