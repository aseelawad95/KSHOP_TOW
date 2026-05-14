
using KSHOP_TOW.PL.Extentions;
using KSHOP_TWO.BLL.MappingConfig;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using KSHOP_TWO.DAL.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Serilog;


namespace KSHOP_TOW.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(
               path: "logs/log-.txt",
               rollingInterval: RollingInterval.Day,
               retainedFileCountLimit: 7,
               outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
           )
           .CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.

            builder.Services.AddControllers();
            MapsterConfig.MapsterConfigRegister();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
             
            // colors
              builder.Services.AddColorsPolicy(MyAllowSpecificOrigins);

            //dbcontext
            builder.Services.AddDatabaseExtentions(builder.Configuration);

            builder.Services.AddLocalization(options => options.ResourcesPath = "");

            const string defaultCulture = "en";

            var supportedCultures = new[]
            {
                 new CultureInfo(defaultCulture),
                 new CultureInfo("ar")
              };

            // Localization
            builder.Services.AddLocalizationExtentions(defaultCulture, supportedCultures);

            // Repositories
            builder.Services.AddRepository(builder.Configuration);

            // Identity
            builder.Services.AddIdentityExtentions();

            //Authentication
            builder.Services.AddJwtAuthentication(builder.Configuration);


            builder.Services.AddScoped<ISeedData, RoleSeedData>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            // Configure the HTTP request pipeline.
            
                app.MapOpenApi();

            app.MapScalarApiReference();

            app.UseHttpsRedirection();

            app.UseStaticFiles(); 

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach(var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }

            app.Run();
        }
    }
}
//    
