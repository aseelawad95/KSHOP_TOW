using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = categoryService;
           _localizer = localizer;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {

            var categories =await _categoryService.GetAll();
            return Ok(new
            {
            data = categories,
            message = _localizer["Success"].Value

            });
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest request)
        {

          var response= await _categoryService.CreateCategories(request);
            return Ok(new
            {
                data = response,
                message = _localizer["Success"].Value
            });
        }

        [HttpGet("{id")]

        public async Task<IActionResult> GetById(int id)
        {
          
            return Ok(await _categoryService.GetCategory(c => c.Id == id));
        }
    }
}


