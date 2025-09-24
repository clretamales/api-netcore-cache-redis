using System.Net;
using System.Text.Json.Serialization;

namespace Arquetipo.Api.Models.Header;

public class HeaderBase
{
    /// <summary>
    /// Version del Servicio
    /// </summary>
    /// <example>2.0</example>
    [JsonPropertyOrder(1)]
    public string Version { get; }
    /// <summary>
    /// Status peticion del servicio
    /// </summary>
    /// <example>200</example>
    [JsonPropertyOrder(2)]
    public int Status { get; set; }
    /// <summary>
    /// Nombre del Host
    /// </summary>
    /// <example>Server01</example>
    [JsonPropertyOrder(3)]
    public string HostName { get; }
    /// <summary>
    /// Nombre del Path de la ruta de la aplicacion
    /// </summary>
    /// <example>C://Carpeta/Carpeta2/CarpetaAplicaciones</example>
    [JsonPropertyOrder(4)]
    public string Path { get; }
    /// <summary>
    /// Codigo de Version del Servicio
    /// </summary>
    /// <example>1</example>
    [JsonPropertyOrder(5)]
    public string CodeVersion { get; }
 
    public HeaderBase()
    {
        Version = "0.1";
        Status = 200;
        HostName = Dns.GetHostName();
        Path = AppContext.BaseDirectory;
        CodeVersion = Environment.Version.Revision.ToString();
    }
}
