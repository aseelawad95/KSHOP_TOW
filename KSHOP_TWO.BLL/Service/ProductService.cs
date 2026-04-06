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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }


        public async Task CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }
            await _productRepository.CreateAsync(product);
        }


        public async Task<List<ProductResponse>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync(
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                }
                );

            return products.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                });
            if (product == null) return null;
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(c => c.Id == id);
            if (product == null) return false;
              _fileService.Delete(product.MainImage);
            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var productDb = await _productRepository.GetOne(c => c.Id == id,
                new string[]
                {
                    nameof(Product.Translations),
                }
                );
            if (productDb == null) return false;
            request.Adapt(productDb);

            if (request.Translations != null)
            {
                foreach (var translation in request.Translations)
                {
                    var existing = productDb.Translations.FirstOrDefault(t => t.Language == translation.Language);
                    if (existing != null)
                    {
                        if (translation.Name != null)
                        {
                            existing.Name = translation.Name;
                        }
                        if (translation.Description != null)
                        {
                            existing.Description = translation.Description;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }



                var oldOmage = productDb.MainImage;
                if (request.MainImage != null)
                {
                    _fileService.Delete(oldOmage);
                    var imagePath = await _fileService.UploadAsync(request.MainImage);
                    productDb.MainImage = imagePath!;

                }
                else
                {
                    productDb.MainImage = oldOmage;
                }

                return await _productRepository.UpdateAsync(productDb);


            }
            return true;
        }
        
    }
}
