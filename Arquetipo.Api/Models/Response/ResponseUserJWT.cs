using Arquetipo.Api.Models.Header;

namespace Arquetipo.Api.Models.Response;

// es el objeto return del controller
public class ResponseUserJWT : HeaderBase
{
    // data encapsula del response, permite retornar
    // objetos incluyendo listas de objetos.
    public UserJWTData UsersJwt { get; set; }
}