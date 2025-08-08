using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Interfaces.Services
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Set<T>(string key, T value, TimeSpan? expiration = null);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        void Remove(string key);
        Task RemoveAsync(string key);
        bool Exists(string key);
        Task<bool> ExistsAsync(string key);
    }
}
