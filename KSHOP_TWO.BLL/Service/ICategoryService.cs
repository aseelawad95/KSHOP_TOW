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
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAll();

        Task<CategoryResponse> CreateCategories(CategoryRequest request);

        Task<CategoryResponse> GetCategory(Expression<Func<Category, bool>> filter);

    }
}
