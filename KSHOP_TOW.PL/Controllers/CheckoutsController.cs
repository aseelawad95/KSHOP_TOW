using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        public CheckoutsController(ICheckoutService checkoutService, IStringLocalizer<SharedResources> localizer)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> PaymentA([FromBody] CheckoutRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _checkoutService.ProcessCheckout(userId, request);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpGet("success")]
        [AllowAnonymous]

        public  async Task<IActionResult> Success([FromQuery] string sessionId)
        {
            var result =await _checkoutService.HandlePaymentSuccess(sessionId);

            return Ok(new
            {
                Message = "Payment successful",
                SessionId = sessionId,

            });
        }
    }
}
