using KSHOP_TWO.DAL.DTO.Response;
using Real_Estate.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate.BLL.Services.FavoriteSer
{
    public interface IFavoriteService
    {
        Task<bool> ToggleFavorite(int productId, string userId);
        Task<List<FavoriteResponse>> GetUserFavorites(string userId);
    }
}
