using KSHOP_TWO.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Models
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

       
        public List<CategoryTranslation> Translations { get; set; }

        public List<Product> products { get; set; }

    }
}
