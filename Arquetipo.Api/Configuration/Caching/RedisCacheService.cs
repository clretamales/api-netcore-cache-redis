using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Arquetipo.Api.Configuration.Caching;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache,
                            ILogger<RedisCacheService> logger)
    {
        _cache  = cache;
        _logger = logger;
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken ct = default)
    {
        _logger.LogInformation("Getting cache item with key: {Key}", key);
        var bytes = await _cache.GetAsync(key, ct);
        return bytes is null ? default : JsonSerializer.Deserialize<T>(bytes);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default)
    {
        
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        var opts = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
        await _cache.SetAsync(key, bytes, opts, ct);
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        return _cache.RemoveAsync(key, ct);
    }
}