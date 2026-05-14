using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.MappingConfig
{
    public class MapsterConfig
    {

      static  public void MapsterConfigRegister() {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.category_id, source => source.Id)
               .Map(dest => dest.MainImage, source => source.MainImage)
                .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
               .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()

               )
               .Map(dest => dest.MainImage, source => $"https://localhost:7082/images/{source.MainImage}")
               .Map(dest=> dest.SubImages ,src => src.ProductImages.Select(i => $"https://localhost:7082/images/{i.ImagePath}"));


            TypeAdapterConfig<Brand, BrandResponse>.NewConfig()
              .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()

              )
              .Map(dest => dest.Logo, source => $"https://localhost:7082/images/{source.Logo}");

            TypeAdapterConfig<ProductUpdateRequest, Product>.NewConfig()
                 .IgnoreNullValues(true);

            TypeAdapterConfig<BrandUpdateRequest, Brand>.NewConfig()
                 .IgnoreNullValues(true);


            TypeAdapterConfig<Cart, CartResponse>.NewConfig()
              .Map(dest => dest.ProductName, source => source.Product.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()

              )
              .Map(dest => dest.ProductImage, source => $"https://localhost:7082/images/{source.Product.MainImage}")
              .Map(dest => dest.Price, source => source.Product.Price)
              ;

            TypeAdapterConfig<OrderItem, OrderItemsResponse>.NewConfig()
              .Map(dest => dest.ProductName, source => source.Product.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()

              );

        }
    }
}
