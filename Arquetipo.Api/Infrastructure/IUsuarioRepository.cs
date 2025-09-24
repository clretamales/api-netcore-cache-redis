using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;

namespace Arquetipo.Api.Infrastructure;

public interface IUsuarioRepository
{
    Task<UsuarioDTO> GetUserByIdAsync(int? id);
    Task PostUsersAsync(List<UsuarioPost> param);
    Task UpdateUsersAsync(UsuarioUpdate param);
    Task DeleteUsersAsync(int? id);
}