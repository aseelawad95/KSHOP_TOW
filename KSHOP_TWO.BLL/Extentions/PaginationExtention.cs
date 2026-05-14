using KSHOP_TWO.DAL.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Extentions
{
    public static class PaginationExtention
    {
        public static async Task<PaginstionPesponse<T>> ToPaginationAsync<T> (this IQueryable<T> query,int page, int limit)
        {
            var totalCount = await query.CountAsync();
            var data = await query.Skip(page - 1).Take(limit).ToListAsync();

            return new PaginstionPesponse<T>
            {
                Data = data,
                Page = page,
                Limit = limit,
                TotalCount = totalCount
            };

        }
    }
}
