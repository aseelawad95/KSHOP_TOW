using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request);

        Task<List<ProductResponse>> GetAllProducts();
    }
}
