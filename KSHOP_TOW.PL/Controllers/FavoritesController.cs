using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estate.BLL.Services.FavoriteSer;
using System.Security.Claims;

namespace Real_Estate.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [Authorize]
        [HttpPost("{productId}/toggle")]
        public async Task<IActionResult> AddFovorite (int productId, [FromQuery] string userId)
        {
            var customerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isFavorited = await _favoriteService.ToggleFavorite(productId, userId);
            return Ok(new { isFavorited });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserFavorites(string userId)
        {
            var favorites = await _favoriteService.GetUserFavorites(userId);
            return Ok(new { data = favorites });
        }
    }
}
