using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Request
{
    public class BrandUpdateRequest
    {
        public IFormFile? Logo { get; set; }
        public List<BrandTranslationRequest>? Translations { get; set; }

        public int? ProductId { get; set; }
    }
}
