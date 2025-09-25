namespace Arquetipo.Api.Configuration.Caching;
public interface IRedisCacheService
{
    Task<T> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
}