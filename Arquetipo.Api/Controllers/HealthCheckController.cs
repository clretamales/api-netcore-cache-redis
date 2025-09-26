using Arquetipo.Api.Configuration;
using Arquetipo.Api.Configuration.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Arquetipo.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly IOptionsSnapshot<ConnectionStrings> _options;

    private readonly IDistributedCache _cache;
    public HealthCheckController(IOptionsSnapshot<ConnectionStrings> options,
                            IDistributedCache cache)
    {
        _options = options;
        _cache = cache;
    }

    [HttpGet("db")]
    public async Task<IActionResult> Db()
    {
        var connectionString = _options.Value.ConexionMySql;
        try
        {
            // await using var conn = new OracleConnection(connectionString);
            await using var conn = new MySqlConnection(connectionString);
            // Sólo abrimos la conexión
            await conn.OpenAsync();
            // Si no lanza excepción, todo bien
            return Ok(new { status = "Healthy" , component = "mysql"});
        }
        catch (MySqlException mex) // OracleException
        {
            // Log mex.Number y mex.Message
            return StatusCode(503, new
            {
                status = "Unhealthy",
                errorCode = mex.Number,
                message = mex.Message
            });
        }
        catch (TimeoutException)
        {
            // Conexión agotó tiempo
            return StatusCode(503, new
            {
                status = "Unhealthy",
                error = "Connection timeout"
            });
        }
        catch (Exception ex)
        {
            // Error al abrir → servicio no disponible
            return StatusCode(503, new { status = "Unhealthy", error = ex.Message });
        }
    }

    [HttpGet("redis")]
    public async Task<IActionResult> Redis()
    {
        try
        {
            var key = "health:ping";
            await _cache.SetStringAsync(key, "pong",
                                    new DistributedCacheEntryOptions {
                                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
                                    });
            var value = await _cache.GetStringAsync(key);
            if (value == "pong") return Ok(new { status = "Healthy", component = "redis" });
            return StatusCode(503, new { status = "Unhealthy", component = "redis", error = "get/set failed" });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { status = "Unhealthy", component = "redis", error = ex.Message });
        }
    }

    // opcional: combo
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var db = await Db() as ObjectResult;
        var rd = await Redis() as ObjectResult;
        var allHealthy = db?.StatusCode == 200 && rd?.StatusCode == 200;
        return StatusCode(allHealthy ? 200 : 503, new
        {
            status = allHealthy ? "Healthy" : "Unhealthy",
            results = new[] { db?.Value, rd?.Value }
        });
    }
} 
 
        

