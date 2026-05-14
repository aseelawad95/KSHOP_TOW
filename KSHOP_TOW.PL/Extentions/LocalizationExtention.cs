using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace KSHOP_TOW.PL.Extentions
{
    public static class LocalizationExtention
    {
        public static IServiceCollection AddLocalizationExtentions(this IServiceCollection Services ,
            string defaultCulture = "en",
            CultureInfo[]? supportedCultures = null
            )
        {
            Services.AddLocalization();

            Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Clear();
                //options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                //{
                //    QueryStringKey = "lang"
                //});
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });
            return Services;
        }
    }
}
