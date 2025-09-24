using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;

namespace Arquetipo.Api.Handlers;

public interface IUsuarioHandler
{
    Task<DataUsuario> GetUsuarioAsync(int? id);
    Task InsertUsuariosAsync(List<UsuarioPost> request);
}