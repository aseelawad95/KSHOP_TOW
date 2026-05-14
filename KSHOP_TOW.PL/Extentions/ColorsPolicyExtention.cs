namespace KSHOP_TOW.PL.Extentions
{
    public static class ColorsPolicyExtention
    {
        public static IServiceCollection AddColorsPolicy(this IServiceCollection Services,
            string MyAllowSpecificOrigins = "AllowAll"
            )
            {
            Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      //policy.WithOrigins("http://example.com",
                                      //                    "http://www.contoso.com");
                                      policy.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader();
                                  });
            });

            return Services;
        }
    }
}
