using KSHOP_TWO.DAL.Data;
using KSHOP_TWO.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Repository
{
    public class Genericrepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public Genericrepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public Task<T> CreateAsync(Category categpry)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync(string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }


        public async Task<T> GetOne(Expression<Func<T, bool>> filter,string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(filter);
        }



    }
}
