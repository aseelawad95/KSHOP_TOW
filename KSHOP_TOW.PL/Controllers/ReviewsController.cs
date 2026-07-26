using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IRewiewService _rewiewService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ReviewsController(IRewiewService rewiewService, IStringLocalizer<SharedResources> localizer)
        {
            _rewiewService = rewiewService;
            _localizer = localizer;
        }


        [HttpPost("{userId}")]
        public async Task<IActionResult> AddReviews([FromBody] AddReviewRequest request , string userId)
        {
            var result = await _rewiewService.AddReview(userId, request);

            if (!result)
                return BadRequest("Cannot add review");

            return Ok("Review added successfully");
        }
    }
}
