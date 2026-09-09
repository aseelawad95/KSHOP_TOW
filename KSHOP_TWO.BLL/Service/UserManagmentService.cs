using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class UserManagmentService : IUserManagmentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagmentService(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> ChangeRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roleExtists = await _roleManager.RoleExistsAsync(role);
            if (!roleExtists) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> EditUser(UserEditRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "المستخدم غير موجود"
                });

            if (!string.IsNullOrEmpty(request.UserName))
                user.UserName = request.UserName;

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<List<UserListResponse>> GetAllUser()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Adapt<List<UserListResponse>>();
        }

        public async Task<UserDetailsResponse?> GetUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = user.Adapt<UserDetailsResponse>();
            result.Role = roles.FirstOrDefault();

            return result;
        }

        public async Task<bool> ToggleBlockedUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            bool isBlocked = user.LockoutEnd > DateTime.UtcNow;
            if(isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);

            }
            else
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));

            }
            return true;
        }
    }
}
