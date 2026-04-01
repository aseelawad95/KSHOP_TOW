using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface IAuthenticationService
    {

        Task<RegisterResponse> RegisterAsync(RegisterRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<bool> ConfirmdEmailAsync(string userId,string token);

        Task<ForgotPasswordResponse> RequestPasswordRestAsync(ForgotPasswordRequest request);

        Task<RestPasswordResponse> RestPasswordAsync(RestPasswordRequest request);
    }
}
