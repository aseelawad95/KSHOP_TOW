using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class ProductRepository : Genericrepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Product>?> DecreaseQuantityAsync(List<OrderItem> orderItems)
        {
            var productIds = orderItems.Select(oi => oi.ProductId).ToList();

            var products = await GetAllAsync(p => productIds.Contains(p.Id));

            foreach (var product in products)
            {
                var item = orderItems.FirstOrDefault(oi => oi.ProductId == product.Id);
                product.Quantity -= item.Quantity;
            }

            
            await UpdateRangeAsync(products);
            return products.Where(p => p.Quantity <5).ToList();
        }
    }
}
