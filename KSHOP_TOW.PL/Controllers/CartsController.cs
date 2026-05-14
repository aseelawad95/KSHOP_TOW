using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CartsController(ICartService cartService, IStringLocalizer<SharedResources> localizer)
        {

            _cartService = cartService;
            _localizer = localizer;
        }

        [HttpPost("")]

        public async Task<IActionResult> AddTCart(AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCart(request, userId);
            if (!result) return BadRequest();
            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }


        [HttpGet("")]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.GetCart(userId);
            return Ok(result);
        }


        [HttpPatch("{ProductId}")]

        public async Task<IActionResult> UpdateQuantity([FromRoute] int ProductId, [FromBody] UpdateCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.UpdateQuantity(ProductId, request.Count, userId);
            if (!result) return BadRequest();
            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveItem(id, userId);
            if (!result) return BadRequest();
            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }
    }
}
