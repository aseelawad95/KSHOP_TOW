using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Migrations;
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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository  _brandRepository;
        private readonly IFileService _fileService;

        public BrandService(IBrandRepository brandRepository, IFileService fileService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
        }

        public async Task CreateBrand(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();
            if (request.Logo != null)
            {
                var imagePath = await _fileService.UploadAsync(request.Logo);
                brand.Logo = imagePath;
            }
            await _brandRepository.CreateAsync(brand);
        }

       
        public async Task<List<BrandResponse>> GetAllBrands()
        {
            var brands = await _brandRepository.GetAllAsync(
              new string[]
              {
                    nameof(Brand.Translations),
                                         
              }
              );

            return brands.Adapt<List<BrandResponse>>();
        }

        public async Task<BrandResponse> GetBrand(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _brandRepository.GetOne(filter,
              new string[]
              {
                    nameof(Brand.Translations),
              });
            if (brand == null) return null;
            return brand.Adapt<BrandResponse>();
        }

        public async Task<bool> DeleteBrand(int id)
        {
            var brand = await _brandRepository.GetOne(c => c.Id == id);
            if (brand == null) return false;
            _fileService.Delete(brand.Logo);
            await _brandRepository.DeleteAsync(brand);
            return true;
        }

        public async Task<bool> UpdateBrand(int id, BrandUpdateRequest request)
        {
            var brand = await _brandRepository.GetOne(b => b.Id == id ,
                new string[]
              {
                    nameof(Brand.Translations),
              });
            if (brand == null) return false;

            request.Adapt(brand);

            if (request.Translations != null)
            {
                foreach (var translation in request.Translations)
                {
                    var existing = brand.Translations.FirstOrDefault(t => t.Language == translation.Language);
                    if (existing != null)
                    {
                        if (!string.IsNullOrEmpty(translation.Name))
                        {
                            existing.Name = translation.Name;
                        }

                        else
                        {
                            return false;
                        }

                    }
                }
                var oldImage = brand.Logo;
                if (request.Logo != null)
                {
                    _fileService.Delete(oldImage);
                    var imagePath = await _fileService.UploadAsync(request.Logo);
                    brand.Logo = imagePath;
                }
                else
                {
                    brand.Logo = oldImage;
                }
                return await _brandRepository.UpdateAsync(brand);


            }
            return true;
        }
    }
}
