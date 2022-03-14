using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Core.Paging
{
    public class PagingResult<T>  
    {
        public PagingMetaData MetaData { get; private set; }
        public IEnumerable<T> Items { get; private set; }

        public PagingResult(List<T> items, int count, int pageSize, int pageNumber)
        {
            MetaData = new PagingMetaData(count, pageSize, pageNumber);
            Items = items;
        }

        public static async Task<PagingResult<T>> CreateAsync(IQueryable<T> source, PagingParams pagingParams)
        {
            var all =  await source.CountAsync();
            var items = await source.Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                           .Take(pagingParams.PageSize)
                           .ToListAsync();

            return   new PagingResult<T>(items, all, pagingParams.PageSize, pagingParams.PageNumber);
        }


    }
}
