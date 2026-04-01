using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result =await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("ConfirmEmail")]

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            var isConfirmed = await _authenticationService.ConfirmdEmailAsync(userId,token);
            if (!isConfirmed)
                return BadRequest(new { Message = "Invalid email or token" });
            return Ok(new {Message = "Email confirmed" });
        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> RequsetPasswordRest(ForgotPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordRestAsync(request);

            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("RestPassword")]
        public async Task<IActionResult> RestPassword(RestPasswordRequest request)
        {
            var result = await _authenticationService.RestPasswordAsync(request);

            if (!result.Success) return BadRequest(result);

            return Ok(result);

        }
    }
}
