using System.ComponentModel.DataAnnotations;

namespace Arquetipo.Api.Configuration;

public class ConnectionStrings
{
    public const string Seccion = "ConnectionStrings";
    [Required]
    public required string ConexionBD { get; set; }
    [Required]
    public required string ConexionMySql { get; set; }
}
