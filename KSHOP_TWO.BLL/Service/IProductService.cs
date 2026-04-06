using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request);

        Task<List<ProductResponse>> GetAllProducts();

        Task<ProductResponse> GetProduct(Expression<Func<Product, bool>> filter);

        Task<bool> DeleteProduct(int id);

        Task<bool> UpdateProduct(int id, ProductUpdateRequest request);
    }
}
