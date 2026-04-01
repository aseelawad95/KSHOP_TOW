using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class ProductRepository : Genericrepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
