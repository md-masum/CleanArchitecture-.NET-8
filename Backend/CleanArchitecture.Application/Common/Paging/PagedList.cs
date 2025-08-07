using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Paging
{
    public class PagedList<TEntity>(List<TEntity> items, int count, int pageNumber, int pageSize)
        : List<TEntity>
    {
        public int CurrentPage { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int TotalCount { get; set; } = count;
        public IList<TEntity> Data { get; set; } = items;

        public static async Task<PagedList<TEntity>> CreateAsync(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<TEntity>(items, count, pageNumber, pageSize);
        }
    }
}
