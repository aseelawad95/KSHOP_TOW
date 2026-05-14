using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public interface IProductRepository : IGenericRepository<Product>
    {
    Task<List<Product>?> DecreaseQuantityAsync(List<OrderItem> orderItems);
    }

