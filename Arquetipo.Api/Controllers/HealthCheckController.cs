using Arquetipo.Api.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Arquetipo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IOptionsSnapshot<ConnectionStrings> options;
        public HealthCheckController(IOptionsSnapshot<ConnectionStrings> options)
        {
            this.options = options;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var connectionString = options.Value.ConexionMySql;
            try
            {
                // await using var conn = new OracleConnection(connectionString);
                await using var conn = new MySqlConnection(connectionString);
                // Sólo abrimos la conexión
                await conn.OpenAsync();
                // Si no lanza excepción, todo bien
                return Ok(new { status = "Healthy" });
            }
            catch (MySqlException mex) // OracleException
            {
                // Log mex.Number y mex.Message
                return StatusCode(503, new {
                    status    = "Unhealthy",
                    errorCode = mex.Number,
                    message   = mex.Message
                });
            }
            catch (TimeoutException )
            {
                // Conexión agotó tiempo
                return StatusCode(503, new { 
                    status  = "Unhealthy",
                    error   = "Connection timeout"
                });
            }
            catch (Exception ex)
            {
                // Error al abrir → servicio no disponible
                return StatusCode(503, new { status = "Unhealthy", error = ex.Message });
            }
        }
    }
}
