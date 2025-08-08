using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Shared.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache? _cache;

        public RedisCacheService(IDistributedCache? cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key)
        {
            if (_cache == null) return default;
            var value = _cache.GetString(key);
            if (string.IsNullOrEmpty(value)) return default;
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (_cache == null) return default;
            var value = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(value)) return default;
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (_cache == null) return;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            };
            var jsonValue = System.Text.Json.JsonSerializer.Serialize(value);
            _cache.SetString(key, jsonValue, options);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            if (_cache == null) return;
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            };
            var jsonValue = System.Text.Json.JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonValue, options);
        }

        public void Remove(string key)
        {
            if (_cache == null) return;
            _cache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            if (_cache == null) return;
            await _cache.RemoveAsync(key);
        }

        public bool Exists(string key)
        {
            if (_cache == null) return false;
            var value = _cache.GetString(key);
            return !string.IsNullOrEmpty(value);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            if (_cache == null) return false;
            var value = await _cache.GetStringAsync(key);
            return !string.IsNullOrEmpty(value);
        }

    }
}
