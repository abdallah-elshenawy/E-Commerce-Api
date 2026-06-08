
using E_Commerce.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (json is null)
                    return default;

                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GET failed for key {Key}", key);
                return default;
            }
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
                };
                var json = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(key, json, options);
            }
            catch (Exception ex)
            {
                _logger.LogError("Redis SET faild for key {Key}", key);
            }
        }
        public async Task RemoveAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError("Redis REMOVE faild for key {Key}", key);
            }
        }

    }
}
