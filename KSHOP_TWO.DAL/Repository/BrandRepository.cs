using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class BrandRepository : Genericrepository<Brand>, IBrandRepository

    {
        public BrandRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
