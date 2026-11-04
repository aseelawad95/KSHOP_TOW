using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.Repository;
using Real_Estate.BLL.Services.FavoriteSer;
using Real_Estate.DAL.Repository.FavoriteRepo;
using Stripe;

namespace KSHOP_TOW.PL.Extentions
{
    public static class RepositoryExtention
    {
        public static IServiceCollection AddRepository(this IServiceCollection Services, IConfiguration Configuration)
            {
            Services.AddScoped<ICategoryRepository, Categoryrepository>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IAuthenticationService, AuthenticationService>();
            Services.AddScoped<IFileService, KSHOP_TWO.BLL.Service.FileService>();
            Services.AddScoped<IProductService, KSHOP_TWO.BLL.Service.ProductService>();
            Services.AddScoped<IProductRepository, ProductRepository>();
            Services.AddScoped<IBrandService, BrandService>();
            Services.AddScoped<IBrandRepository, BrandRepository>();
            Services.AddScoped<ICartRepository, CartRepository>();
            Services.AddScoped<ICartService, CartService>();
            Services.AddScoped<ICheckoutService, KSHOP_TWO.BLL.Service.CheckoutService>();
            Services.AddScoped<IOrderRepository, OrderRepository>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IUserManagmentService, UserManagmentService>();
            Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            Services.AddScoped<IFavoriteService, FavoriteService>();
            Services.AddScoped<IReviewRepository, ReviewRepository>();
            Services.AddScoped<IRewiewService, KSHOP_TWO.BLL.Service.ReviewService>();

            Services.AddScoped<INotificationRepository, NotificationRepository>();
            Services.AddScoped<IDeviceTokenRepository, DeviceTokenRepository>();
            Services.AddScoped<INotificationService, NotificationService>();

            Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];

            return Services;

        } 
    }
}
