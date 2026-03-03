using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryrepository;
        public CategoryService(ICategoryRepository categoryrepository)
        {
            _categoryrepository = categoryrepository;
        }
        public async Task<CategoryResponse> CreateCategories(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
          await  _categoryrepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories =await _categoryrepository.GetAllAsync(new string[] {nameof(Category.Translations)});

            return categories.Adapt<List<CategoryResponse>>();
        }

       public async Task<CategoryResponse> GetCategory(Expression<Func<Category,bool>> filter)
        {
            var category = await _categoryrepository.GetOne(filter,new string[] {nameof(Category.Translations)});
            return category.Adapt<CategoryResponse>();
            
        }
    }
}
