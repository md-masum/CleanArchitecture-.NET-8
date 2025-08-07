using CleanArchitecture.Application.Common.Paging;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories.Examples
{
    public class ExampleRepository(ApplicationDbContext context) : BaseRepository<Example>(context), IExampleRepository
    {
        private readonly ApplicationDbContext _context = context;

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<PagedList<Example>> GetAllExample(int pageNumber, int pageSize)
        {
            var examples = GetAsQueryable();
            return await PagedList<Example>.CreateAsync(examples, pageNumber, pageSize);
        }

        public async Task<Example> GetExampleById(int id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<Example> CreateExample(Example example)
        {
            await AddAsync(example);
            return example;
        }

        public async Task<Example> UpdateExample(Example example)
        {
            await UpdateAsync(example);
            return example;
        }

        public async Task<bool> DeleteExample(Example example)
        {
            return await RemoveAsync(example);
        }

        public async Task<bool> IsUniqueEmailAsync(string email)
        {
            var example = await _context.Examples.FirstOrDefaultAsync(c => c.Title3 == email);
            return example is null;
        }

        public async Task<bool> IsUniqueEmailForUpdateAsync(int id, string email)
        {
            var example = await _context.Examples.FirstOrDefaultAsync(c => c.Title3 == email && c.Id != id);
            return example is null;
        }
    }
}
