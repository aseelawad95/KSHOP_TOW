namespace KSHOP_TOW.PL.Extentions
{
    public static class ColorsPolicyExtention
    {
        public static IServiceCollection AddColorsPolicy(this IServiceCollection Services,
            string MyAllowSpecificOrigins = "AllowSpecificOrigins"
            )
        {
            Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins(
                                          "https://yourfrontend.com",
                                          "https://www.yourfrontend.com"
                                      // ضيف أي دومين تاني بدك تسمحلو
                                      )
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                                  });
            });
            return Services;
        }
    }
}