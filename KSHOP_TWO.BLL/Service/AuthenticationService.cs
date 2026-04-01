using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ForgotPasswordRequest = Microsoft.AspNetCore.Identity.Data.ForgotPasswordRequest;
using LoginRequest = KSHOP_TWO.DAL.DTO.Request.LoginRequest;

namespace KSHOP_TWO.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(UserManager<ApplicationUser> userManger, IEmailSender emailSender, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManger;
            _emailSender = emailSender;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<RegisterResponse> RegisterAsync(DAL.DTO.Request.RegisterRequest request)
        {
            var user = request.Adapt<ApplicationUser>();
            var result = await _userManager.CreateAsync(user, request.Password);
            
            foreach(var error in result.Errors)
            {
                Console.WriteLine(error.Code);
                Console.WriteLine(error.Description);
            }




            if (!result.Succeeded)
                return new RegisterResponse()
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Success = false,
                    Errors = result.Errors.Select(p => p.Description).ToList()
                };
            await _userManager.AddToRoleAsync(user, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = Uri.EscapeDataString(token);
            var emailUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/api/account/ConfirmEmail?token={token}&userId={user.Id}";

            await _emailSender.SendEmailAsync(
                user.Email,
                "Welcome",
                $"<h1> welcome {request.UserName} </h1>" +
                $"<a href='{emailUrl}'>confirm</a>"
            );

            return new RegisterResponse()
            {
                Success = true,
                Message = "success"
            };

        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)

                return new LoginResponse() { Message = "User not found", Success = false };

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new LoginResponse() { Message = "User not Confirmed", Success = false };
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return new LoginResponse() { Message = "Invalid password", Success = false };
            return new LoginResponse() { Message = "Success", Success = true, AccessToken = await GenerateAccessToken(user) };
        }

        private Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: userClaims,
        expires: DateTime.Now.AddDays(5),
        signingCredentials: credentials
        );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));

        }


        public async Task<bool> ConfirmdEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return false;
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return false;
            return true;
        }

        //public async Task<ForgotPasswordResponse> RequestPasswordRestAsync(ForgotPasswordRequest request)
        //{
        //    var user = await _userManager.FindByEmailAsync(request.Email);
        //    if (user is null)
        //    {
        //        return new ForgotPasswordResponse()
        //        {
        //            Success = false,
        //            Message = "Invalid Email"
        //        };
        //    }

        //    var random = new Random();
        //    var code = random.Next(1000, 9999).ToString();
        //    user.CodeRestPassword = code;
        //    user.PasswordRestCodeExpiry = DateTime.Now.AddMinutes(15);

        //    await _userManager.UpdateAsync(user);

        //    await _emailSender.SendEmailAsync(request.Email, "Rest Password", $"<p>The code is {code}</p>");
        //    return new ForgotPasswordResponse()
        //    {
        //        Success = true,
        //        Message = "Send THe code"
        //    };

        //}


        public async Task<RestPasswordResponse> RestPasswordAsync(RestPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new RestPasswordResponse()
                {
                    Success = false,
                    Message = "Email Not Found"
                };
            }
            else if (user.CodeRestPassword != request.Code)
            {
                return new RestPasswordResponse()
                {
                    Success = false,
                    Message = "invalid code"
                };
            }
            else if (user.PasswordRestCodeExpiry < DateTime.UtcNow)
            {
                return new RestPasswordResponse()
                {
                    Success = false,
                    Message = "Code Expired"
                };
            }
      var isSamePassword =       await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (isSamePassword)
            {
                return new RestPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Password",
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!result.Succeeded)
            {
                return new RestPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Rest Password",
                };
            }
            await _emailSender.SendEmailAsync(request.Email, "Change Password", $"<p>Tour password changed</p>");
            return new RestPasswordResponse()
            {
                Success = true,
                Message = "Success"
            };
        }

        public async Task<ForgotPasswordResponse> RequestPasswordRestAsync(DAL.DTO.Request.ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ForgotPasswordResponse()
                {
                    Success = false,
                    Message = "Invalid Email"
                };
            }

            var random = new Random();
            var code = random.Next(1000, 9999).ToString();
            user.CodeRestPassword = code;
            user.PasswordRestCodeExpiry = DateTime.Now.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            await _emailSender.SendEmailAsync(request.Email, "Rest Password", $"<p>The code is {code}</p>");
            return new ForgotPasswordResponse()
            {
                Success = true,
                Message = "Send THe code"
            };

        }
    }

    }
