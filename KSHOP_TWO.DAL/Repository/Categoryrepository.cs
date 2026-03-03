using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class Categoryrepository : Genericrepository<Category> , ICategoryRepository
    {

        public Categoryrepository(ApplicationDbContext context) : base(context)
        {

        }

    }
}
