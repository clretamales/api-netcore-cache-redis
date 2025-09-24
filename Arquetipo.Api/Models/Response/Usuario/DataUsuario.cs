using System.Text.Json.Serialization;
using Arquetipo.Api.Models.Header;

namespace Arquetipo.Api.Models.Response.Usuario;

public class DataUsuario : HeaderBase
{
    // data encapsula del response, permite retornar
    // objetos incluyendo listas de objetos.
    [JsonPropertyOrder(10)]
    public Usuario Data { get; set; }
}
