using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(string[]? includes = null);

        Task<T> CreateAsync(T categpry);

        Task<T> GetOne(Expression<Func<T, bool>> filter, string[]? includes = null);
    }
}
