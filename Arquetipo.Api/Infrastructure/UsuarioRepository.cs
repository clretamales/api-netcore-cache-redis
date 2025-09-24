using Arquetipo.Api.Configuration;
using Arquetipo.Api.Models.Request;
using Arquetipo.Api.Models.Response.Usuario;
using Dapper;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Arquetipo.Api.Infrastructure;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ILogger<UsuarioRepository> _logger;
    private readonly IOptionsSnapshot<ConnectionStrings> options;
    public UsuarioRepository(ILogger<UsuarioRepository> logger,
                            IOptionsSnapshot<ConnectionStrings> options)
    {
        _logger = logger;
        this.options = options;
    }

    public async Task<UsuarioDTO> GetUserByIdAsync(int? id)
    {
        var connectionString = options.Value.ConexionMySql;
        var respuesta = new UsuarioDTO();
        var sql = @"
                SELECT 
                    id,
                    nombre_usuario AS NombreUsuario,
                    email
                FROM usuario
                WHERE id = @Id;
            ";

        await using var conn = new MySqlConnection(connectionString);
        respuesta = await conn.QueryFirstOrDefaultAsync<UsuarioDTO>(sql, new { Id = id });
        return respuesta;
    }

    public async Task PostUsersAsync(List<UsuarioPost> param)
    {
        var connectionString = options.Value.ConexionMySql;

        var sql = @"
                INSERT INTO usuario (nombre_usuario, email)
                VALUES (@nombres, @mail) ;
            ";

        await using var conn = new MySqlConnection(connectionString);
        await conn.ExecuteAsync(sql, param);

    }

    public async Task UpdateUsersAsync(UsuarioUpdate param)
    {
        var connectionString = options.Value.ConexionMySql;

        var sql = @"
                UPDATE usuario 
                Set nombre_usuario = @nombres,
                    email = @mail            
                WHERE id = @Id;
            ";

        await using var conn = new MySqlConnection(connectionString);
        await conn.ExecuteAsync(sql, param);
        
    }
    
    public async Task DeleteUsersAsync(int? id)
    {
        var connectionString = options.Value.ConexionMySql;

        var sql = @"
                DELETE FROM usuario 
                WHERE id = @Id; 
            ";

        await using var conn = new MySqlConnection(connectionString);
        await conn.ExecuteAsync(sql, new { Id = id });

    }

}