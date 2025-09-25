using Arquetipo.Api.Handlers;
using Arquetipo.Api.Infrastructure;
using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Arquetipo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioHandler _usuarioHandler;
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(ILogger<UsuarioController> logger,
                                IUsuarioHandler usuarioHandler,
                                IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioHandler = usuarioHandler;
            _usuarioRepository = usuarioRepository;
        }

        /// <summary>Obtiene Usuarios</summary>
        /// <remarks>V1 Permite Obtener Lista de Usuarios</remarks>
        [MapToApiVersion("1")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataUsuario))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetUser(int? id)
        {
            _logger.LogInformation("Getting user by id: {Id}", id);
            
            var user = await _usuarioHandler.GetUsuarioAsync(id);
            if (user.Data is null) return NoContent();
            return Ok(user);
        }

        /// <summary>Inserta Usuarios</summary>
        /// <remarks>V1 Permite insertar usuarios</remarks>
        [MapToApiVersion("1")]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> PostUsers([FromBody] List<UsuarioPost> request)
        {
            await _usuarioHandler.InsertUsuariosAsync(request);
            
            return Created();
        }

        /// <summary>Actualiza Usuario</summary>
        /// <remarks>V1 Permite Actualizar usuario segun su id</remarks>
        [MapToApiVersion("1")]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutUser([FromBody] UsuarioUpdate user)
        {
            await _usuarioRepository.UpdateUsersAsync(user);
                     
            return NoContent();
        }

        /// <summary>Elimina un Usuario</summary>
        /// <remarks>V1 Permite Eliminar usuario segun su id</remarks>
        [MapToApiVersion("1")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            await _usuarioRepository.DeleteUsersAsync(id);
            
            return NoContent();
        }

    }
}
