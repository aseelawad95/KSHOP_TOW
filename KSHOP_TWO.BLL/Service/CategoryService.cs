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
        private readonly IFileService _fileService;
        public CategoryService(ICategoryRepository categoryrepository, IFileService fileService)
        {
            _categoryrepository = categoryrepository;
            _fileService = fileService;
        }
        public async Task<CategoryResponse> CreateCategories(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                category.MainImage = imagePath;
            }
            await  _categoryrepository.CreateAsync(category);
            var response = category.Adapt<CategoryResponse>();

            response.MainImage = _fileService.GetImageUrl(category.MainImage);
            return response;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category =await _categoryrepository.GetOne(c => c.Id == id);
            if (category == null) return false;
            return await _categoryrepository.DeleteAsync(category);
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
                    var categories = await _categoryrepository.GetAllAsync(
               c => c.Status == EntityStatus.Active,
               new string[] {
                    nameof(Category.Translations),
                    nameof(Category.CreatedBy)
               });

            var response = categories.Adapt<List<CategoryResponse>>();

            foreach (var item in response)
            {
                if (!string.IsNullOrEmpty(item.MainImage))
                {
                    item.MainImage = _fileService.GetImageUrl(item.MainImage);
                }
            }

            return response;
            //categories.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<CategoryResponse>>()

        }

       public async Task<CategoryResponse> GetCategory(Expression<Func<Category,bool>> filter)
        {
            var category = await _categoryrepository.GetOne(filter,new string[] {nameof(Category.Translations)});
            var response = category.Adapt<CategoryResponse>();
            response.MainImage = _fileService.GetImageUrl(category?.MainImage);
            return response;      
        }

       public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest request)
{
    var category = await _categoryrepository
        .GetOne(c => c.Id == id, new string[] {nameof(Category.Translations)});

    if (category == null)
        return null;

    foreach (var translation in request.Translations)
    {
        var existingTranslation = category.Translations
            .FirstOrDefault(t => t.Language == translation.Language);

        if (existingTranslation == null)
            return null;

        existingTranslation.Name = translation.Name;
    }

            await _categoryrepository.UpdateAsync(category);

            return category.Adapt<CategoryResponse>();
}
    }
}
