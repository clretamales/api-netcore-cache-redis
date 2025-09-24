using Arquetipo.Api.Infrastructure;
using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;

namespace Arquetipo.Api.Handlers;

public class UsuarioHandler : IUsuarioHandler
{
    private readonly ILogger<UsuarioHandler> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    public UsuarioHandler(ILogger<UsuarioHandler> logger,
                        IUsuarioRepository usuarioRepository)
    {
        _logger = logger;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<DataUsuario> GetUsuarioAsync(int? id)
    {
        _logger.LogInformation("Iniciando consulta de usuario por id {id}", id);
        var respuesta = new DataUsuario();
        var user = await _usuarioRepository.GetUserByIdAsync(id);
        if (user is not null)
        {
            respuesta.Data = new Usuario();
            respuesta.Data.id = user.id;
            respuesta.Data.nombres = user.nombreUsuario;
            respuesta.Data.mail = user.email;
        }

        return respuesta;
    }

    public async Task InsertUsuariosAsync(List<UsuarioPost> request)
    {   
        await _usuarioRepository.PostUsersAsync(request);   
    }

    
}