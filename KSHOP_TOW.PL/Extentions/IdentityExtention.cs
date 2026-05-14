using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace KSHOP_TOW.PL.Extentions
{
    public static class IdentityExtention
    {
        public static IServiceCollection AddIdentityExtentions(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true; // 0-9
                options.Password.RequireLowercase = true; // a-z
                options.Password.RequireUppercase = true; // A-Z
                options.Password.RequireNonAlphanumeric = true; // ! @ # $ %
                options.Password.RequiredLength = 10;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            return services;
        }
    }
}
