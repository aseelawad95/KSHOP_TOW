using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ProductsController(IProductService productService, IStringLocalizer<SharedResources> localizer)
        {
            _productService = productService;
            _localizer = localizer;
        }

        [HttpGet("")]

        public async Task<IActionResult> Index()
        {
              var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOneProduct(int id)
        {
            var product = await _productService.GetProduct(p=> p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        [HttpPost("")]
        [Authorize]

        public async Task<IActionResult> CreateProduct([FromForm] ProductRequest request)
        {
            await _productService.CreateProduct(request);
            return Ok();

        }

        [HttpPatch("{id}")]
        [Authorize]

        public async Task<IActionResult> UpdateProduct(int id,[FromForm] ProductUpdateRequest request)
        {
         var updated  = await _productService.UpdateProduct(id,request);
            if (!updated)
            {
                return BadRequest();
            }

            return Ok();

        }


        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteProduct(int id)
        {
          var deleted =   await _productService.DeleteProduct(id);
            if (!deleted)
            {
                return BadRequest();
            }
            return Ok();

        }


    }
}
