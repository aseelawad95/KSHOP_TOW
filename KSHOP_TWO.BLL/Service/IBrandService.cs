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
    public interface IBrandService
    {
        Task CreateBrand(BrandRequest request);
        Task<List<BrandResponse>> GetAllBrands();
        Task<BrandResponse> GetBrand(Expression<Func<Brand, bool>> filter);

        Task<bool> DeleteBrand(int id);

        Task<bool> UpdateBrand(int id, BrandUpdateRequest request);
    }
}
