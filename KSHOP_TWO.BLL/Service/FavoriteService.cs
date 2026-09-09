using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using Mapster;
using Real_Estate.DAL.Models;
using Real_Estate.DAL.Repository.FavoriteRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate.BLL.Services.FavoriteSer
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IFileService _fileService;
        public FavoriteService(IFavoriteRepository favoriteRepository, IFileService fileService)
        {
            _favoriteRepository = favoriteRepository;
            _fileService = fileService;
        }

        public async Task<List<FavoriteResponse>> GetUserFavorites(string userId)
        {
            var favorites = await _favoriteRepository.GetAllAsync(
                 l => l.UserId == userId,
                 new string[]
                 {
                    nameof(Favorite.Product),
                    $"{nameof(Favorite.Product)}.{nameof(Product.Translations)}",
                    $"{nameof(Favorite.Product)}.{nameof(Product.ProductImages)}"
                 }
                 );
                var response = favorites.Adapt<List<FavoriteResponse>>();
                foreach (var favorite in response)
                {
                    if (!string.IsNullOrEmpty(favorite.ProductImageUrl))
                    {
                        favorite.ProductImageUrl = _fileService.GetImageUrl(favorite.ProductImageUrl);
                    }
                }
              return response;
            }

        public async Task<bool> ToggleFavorite(int propertyId, string customerId)
        {
            var favorite = await _favoriteRepository.GetOne(
            f => f.ProductId == propertyId && f.UserId == customerId);

            bool isFavorited = favorite != null;

            if (isFavorited)
            {
                await _favoriteRepository.DeleteAsync(favorite);
            }
            else
            {
                var newFavorite = new Favorite
                {
                    ProductId = propertyId,
                    UserId = customerId,
                    CreatedAt = DateTime.Now
                };
                await _favoriteRepository.CreateAsync(newFavorite);
            }

            return !isFavorited;

        }
    }
}
