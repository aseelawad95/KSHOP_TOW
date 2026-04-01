using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
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
           //اول طريقه عشان ابعثله Language 
            //var lang = Request.Headers["Accept-Language"].ToString();
            var categories =await _categoryService.GetAll();
            return Ok(new
            {
            data = categories,
            message = _localizer["Success"].Value

            });
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            //var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
          var response= await _categoryService.CreateCategories(request);
            return Ok(new
            {
                data = response,
                message = _localizer["Success"].Value
            });
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)
        {
          
            return Ok(await _categoryService.GetCategory(c => c.Id == id));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (!deleted)
            {
                return NotFound(new { message = _localizer["NotFound"].Value });
            }
            return Ok(new {message = _localizer["Success"].Value });
        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> Update(int id , CategoryRequest request)
        {
             var response = await _categoryService.UpdateAsync(id, request);

            if (response == null)
            {
                return NotFound(new { message = _localizer["NotFound"].Value });
            }
            return Ok(new
            {
                data = response,
                message = _localizer["Success"].Value
            });
        }
    }
}


