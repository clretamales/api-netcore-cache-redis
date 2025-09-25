namespace Arquetipo.Api.Configuration.Caching;
public class CacheOptions
{
    public const string Seccion = "Cache";
    public int DefaultTtlSeconds { get; set; } = 60;
    public string KeyPrefix { get; set; } = "api-cache:";
}