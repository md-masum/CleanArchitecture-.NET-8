using CleanArchitecture.Application.Common.Paging;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces.Repositories
{
    public interface IExampleRepository : IDisposable
    {
        Task<PagedList<Example>> GetAllExample(int pageNumber, int pageSize);
        Task<Example> GetExampleById(int id);
        Task<Example> CreateExample(Example example);
        Task<Example> UpdateExample(Example example);
        Task<bool> DeleteExample(Example example);
        Task<bool> IsUniqueEmailAsync(string email);
        Task<bool> IsUniqueEmailForUpdateAsync(int id, string email);
    }
}
