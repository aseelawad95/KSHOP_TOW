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

                .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
               .Map(dest => dest.Name, source => source.Translations.Where(t => t.Language == CultureInfo.CurrentCulture.Name).Select(t => t.Name).FirstOrDefault()

               )
               .Map(dest => dest.MainImage, source => $"https://localhost:7082/images/${source.MainImage}");




        }
    }
}
