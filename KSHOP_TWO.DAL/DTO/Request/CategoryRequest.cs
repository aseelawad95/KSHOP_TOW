using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Request
{
    public class CategoryRequest
    {
       public List<CategoryTranslationsRequest> Translations { get; set; }
    }
}
