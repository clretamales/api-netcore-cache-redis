using System.Reflection;
using System.Text.Json;
using Arquetipo.Api.Configuration.Caching;
using Arquetipo.Api.Handlers.Mapper;
using Arquetipo.Api.Infrastructure;
using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Arquetipo.Api.Handlers;

public class UsuarioHandler : IUsuarioHandler
{
    private readonly ILogger<UsuarioHandler> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IDistributedCache _cache;
    private readonly IOptionsSnapshot<CacheOptions> _options;
    private readonly IHttpContextAccessor _http;
    private readonly string _projectName;
    public UsuarioHandler(ILogger<UsuarioHandler> logger,
                        IUsuarioRepository usuarioRepository,
                        IDistributedCache cache,
                        IOptionsSnapshot<CacheOptions> options,
                        IHttpContextAccessor http)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
        _cache = cache;
        _options = options;
        _projectName = Assembly.GetExecutingAssembly().GetName().Name ?? "Arquetipo.Api";
        _http = http;
    }

    public async Task<DataUsuario> GetUsuarioAsync(int? id)
    {
        _logger.LogInformation("Iniciando consulta de usuario por id {id}", id);

        var respuesta = new DataUsuario();
        bool hit = false;

        var key = UsuarioHandlerMapper.BuildKey(_options.Value.KeyPrefix
                                            ,_projectName
                                            ,nameof(GetUsuarioAsync)
                                            ,id.Value);

        // 1) Cache
        var raw = await _cache.GetAsync(key);

        if (raw is not null)
        {
            var cached = JsonSerializer.Deserialize<DataUsuario>(raw);
            if (cached is not null)
            {
                hit = true;
                UsuarioHandlerMapper.SetCacheHeader(_http, hit /*, key*/);
                return cached;
            }
        }

        // 2) Repo        
         _logger.LogInformation("Cache MISS [{Key}] → MySQL", key);

        var user = await _usuarioRepository.GetUserByIdAsync(id);
        if (user is not null)
        {
            respuesta = UsuarioHandlerMapper.MapperUsuario(user);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(respuesta);
            var opts = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromSeconds(_options.Value.DefaultTtlSeconds)
            };
            await _cache.SetAsync(key, bytes, opts);
        }

        UsuarioHandlerMapper.SetCacheHeader(_http, hit /*, key*/);
        return respuesta;
    }

    public async Task InsertUsuariosAsync(List<UsuarioPost> request)
    {   
        await _usuarioRepository.PostUsersAsync(request);   
    }

    
}