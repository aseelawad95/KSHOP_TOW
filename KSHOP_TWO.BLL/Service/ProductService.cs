using KSHOP_TWO.BLL.Extentions;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Real_Estate.BLL.Services.FavoriteSer;
using Real_Estate.DAL.Repository.FavoriteRepo;
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
        private readonly IFavoriteRepository _favoriteRepository;


        public ProductService(IProductRepository productRepository, IFileService fileService, IFavoriteRepository favoriteRepository)
        {
            _productRepository = productRepository;
            _fileService = fileService;
            _favoriteRepository = favoriteRepository;
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


        public async Task<PaginstionPesponse<ProductResponse>> GetAllProducts(ProductFilterRequest request, string? currentUserId)
        {
            var query = _productRepository.GetQuerable(
                p => p.Status == EntityStatus.Active,
                new string[]
                {
                  nameof(Product.Translations),
                  nameof(Product.CreatedBy),
                  nameof(Product.ProductImages),
                });
            if (request.Search != null)
                query = query.Where(p => p.Translations.Any(t => t.Name.Contains(request.Search)));
            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId);
            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice);
            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice);
            if (request.MinRate.HasValue)
                query = query.Where(p => p.Rate >= request.MinRate.Value);
            var paginated = await query.ToPaginationAsync(request.Page, request.Limit);

            var responseData = paginated.Data.Adapt<List<ProductResponse>>();


            HashSet<int> likedProductIds = new HashSet<int>();
            if (!string.IsNullOrEmpty(currentUserId) && paginated.Data.Any())
            {
                var productIds = paginated.Data.Select(p => p.Id).ToList();

                var favorites = await _favoriteRepository.GetAllAsync(
                    f => f.UserId == currentUserId && productIds.Contains(f.ProductId)
                );

                likedProductIds = favorites.Select(f => f.ProductId).ToHashSet();
            }
            // =======================================

            foreach (var product in responseData)
            {
                if (!string.IsNullOrEmpty(product.MainImage))
                    product.MainImage = _fileService.GetImageUrl(product.MainImage);

                if (product.SubImages != null)
                {
                    product.SubImages = product.SubImages
                        .Select(img => !string.IsNullOrEmpty(img) ? _fileService.GetImageUrl(img) : img)
                        .ToList();
                }

                product.IsFavourite = likedProductIds.Contains(product.Id);   
            }
            return new PaginstionPesponse<ProductResponse>
            {
                Data = responseData,
                TotalCount = paginated.TotalCount,
                Page = paginated.Page,
                Limit = paginated.Limit,
            };
        }

        public async Task<ProductGetOneResponse> GetProduct(Expression<Func<Product, bool>> filter, string? currentUserId)
        {
            var product = await _productRepository.GetOne(filter,
                new string[]
                {
                      nameof(Product.Translations),
                        nameof(Product.CreatedBy),
                        nameof(Product.ProductImages),
                        nameof(Product.Reviews),
                });
            if (product == null) return null;
            var response = product.Adapt<ProductGetOneResponse>();
            if (product.ProductImages != null && product.ProductImages.Any())
            {
                response.SubImages = product.ProductImages
                    .Select(img => _fileService.GetImageUrl(img.ImagePath))  
                    .ToList();
            }
            response.MainImage = _fileService.GetImageUrl(product.MainImage);
            if (!string.IsNullOrEmpty(currentUserId))
            {
                var favorite = await _favoriteRepository.GetOne(
                    f => f.ProductId == product.Id && f.UserId == currentUserId
                );
                response.IsFavourite = favorite != null;   
            }
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
            if (request.SubImages != null)
            {
                foreach (var image in productDb.ProductImages)
                {
                    _fileService.Delete(image.ImagePath);
                }
                productDb.ProductImages.Clear();
                foreach (var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    productDb.ProductImages.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }
            if (request.NewImages != null)
            {
                foreach (var image in request.NewImages)
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
