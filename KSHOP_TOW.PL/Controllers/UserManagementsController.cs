using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class UserManagementsController : ControllerBase

    {
        private readonly IUserManagmentService _userManagmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UserManagementsController(IUserManagmentService userManagmentService, IStringLocalizer<SharedResources> localizer)
        {
            _userManagmentService = userManagmentService;
            _localizer = localizer;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManagmentService.GetAllUser();
            return Ok(users);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var user =await _userManagmentService.GetUser(userId);
            return Ok(user);
        }

        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> ChangeRole(string userId, [FromBody] ChangeRoleRequest request)
        {
            var result = await _userManagmentService.ChangeRole(userId, request.NewRole);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpPatch("{userId}/toggle-block")]
        public async Task<IActionResult> ToggleBlockeUser(string userId)
        {
            var result = await _userManagmentService.ToggleBlockedUser(userId);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpPatch("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(string userId, [FromBody] UserEditRequest request)
        {
            request.UserId = userId;

            var result = await _userManagmentService.EditUser(request);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => new { e.Code, e.Description }));

            return Ok("تم تعديل بيانات المستخدم بنجاح");
        }

    }
}
