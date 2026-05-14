using KSHOP_TWO.BLL.Extentions;
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
            if(request.SubImages != null)
            {
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.ProductImages.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }
            var response = await _productRepository.CreateAsync(product);
            response.MainImage = _fileService.GetImageUrl(product.MainImage);
        }


        public async Task<PaginstionPesponse<ProductResponse>> GetAllProducts(PaginationRequest request)
        {
            var query =  _productRepository.GetQuerable(
                p => p.Status == EntityStatus.Active,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.CreatedBy),
                    nameof(Product.ProductImages)
                }
                );
            var paginated = await query.ToPaginationAsync(request.Page, request.Limit);

            var response = query.Adapt<List<ProductResponse>>();

           

            foreach (var product in response)
            {
                if (!string.IsNullOrEmpty(product.MainImage))
                {
                    product.MainImage = _fileService.GetImageUrl(product.MainImage);
                }
            }

            return new PaginstionPesponse<ProductResponse>
            {
                Data = paginated.Data.Adapt<List<ProductResponse>>(),
                TotalCount = paginated.TotalCount,
                Page = paginated.Page,
                Limit = paginated.Limit,
              
            };
            
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
            var response = product.Adapt<ProductResponse>();
            response.MainImage = _fileService.GetImageUrl(product.MainImage);
            return response;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _productRepository.GetOne(c => c.Id == id,
                new string[]
                {
                    nameof(Product.ProductImages),
                }
                );
            if (product == null) return false;
              _fileService.Delete(product.MainImage);
            foreach(var image in product.ProductImages)
            {
                               _fileService.Delete(image.ImagePath);
            }
            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var productDb = await _productRepository.GetOne(c => c.Id == id,
                new string[]
                {
                    nameof(Product.Translations),
                    nameof(Product.ProductImages)
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

                if(request.SubImages != null)
                {
                    foreach(var image in productDb.ProductImages)
                    {
                        _fileService.Delete(image.ImagePath);

                    }
                    productDb.ProductImages.Clear();
                    foreach(var image in request.SubImages)
                    {
                        var imagePath = await _fileService.UploadAsync(image);
                        productDb.ProductImages.Add(new ProductImage
                        {
                            ImagePath = imagePath
                        });
                    }
                }

                if(request.NewImages != null)
                {
                    foreach(var image in request.NewImages)
                    {
                        var imagePath = await _fileService.UploadAsync(image);
                        productDb.ProductImages.Add(new ProductImage
                        {
                            ImagePath = imagePath
                        });
                    }
                }

                return await _productRepository.UpdateAsync(productDb);


            }
            return true;
        }

        public async Task<bool> ToggeleStatus(int id)
        {
            var product = await _productRepository.GetOne(p => p.Id == id);
            if (product == null) return false;
            
            product.Status = product.Status == EntityStatus.Active ?
                EntityStatus.InActive : EntityStatus.Active;
            return await _productRepository.UpdateAsync(product);
        }
        
    }
}
