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
    public class BrandsController : ControllerBase
    {

        private readonly IBrandService _brandService;
        private readonly IStringLocalizer<SharedResources> _localizer;


        public BrandsController(IBrandService brandService, IStringLocalizer<SharedResources> localizer)
        {
            _brandService = brandService;
            _localizer = localizer;
        }

        [HttpGet("")]

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllBrands();

            return Ok(brands);
        }



        [HttpPost("")]
        [Authorize]

        public async Task<IActionResult> CreateBrandt([FromForm] BrandRequest request)
        {
            await _brandService.CreateBrand(request);
            return Ok();

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOneProduct(int id)
        {
            var brand = await _brandService.GetBrand(p => p.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return Ok(brand);
        }



        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] BrandUpdateRequest request)
        {
            try
            {
                var updated = await _brandService.UpdateBrand(id, request);

                if (!updated)
                    return NotFound(new { message = "Brand not found." });

                return Ok(new { message = "Brand updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", detail = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteBrand(int id)
        {
            var deleted = await _brandService.DeleteBrand(id);
            if (!deleted)
            {
                return BadRequest();
            }
            return Ok();

        }


    }
}
