using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Repository;
using Real_Estate.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate.DAL.Repository.FavoriteRepo
{
    public class FavoriteRepository : Genericrepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    
    
}
