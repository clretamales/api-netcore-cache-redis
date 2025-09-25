using Arquetipo.Api.Models.Response.Usuario;
using static System.Net.WebRequestMethods;

namespace Arquetipo.Api.Handlers.Mapper;

public static class UsuarioHandlerMapper
{
    public static string BuildKey(string keyPrefix, string projectName, string metodo, int id)
    {
        return $"{keyPrefix}{projectName}:{metodo}:usuario:{id}";
    }

    public static void SetCacheHeader(IHttpContextAccessor httpContextAccessor, bool hit /*, string key*/)
    {
        var headers = httpContextAccessor.HttpContext?.Response?.Headers;
        if (headers is null) return;
        headers["X-Cache"] = hit ? "HIT" : "MISS";
        // Opcional (debug):
        // headers["X-Cache-Key"] = key;
    }

    public static DataUsuario MapperUsuario(UsuarioDTO user)
    {
        var respuesta = new DataUsuario();
        respuesta.Data = new Usuario();
        respuesta.Data.id = user.id;
        respuesta.Data.nombres = user.nombreUsuario;
        respuesta.Data.mail = user.email;
        return respuesta;
    }
}