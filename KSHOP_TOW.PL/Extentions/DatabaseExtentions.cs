using KSHOP_TWO.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace KSHOP_TOW.PL.Extentions
{
    public static class DatabaseExtentions
    {
        public static IServiceCollection AddDatabaseExtentions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
