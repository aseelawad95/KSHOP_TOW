using KSHOP_TWO.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
