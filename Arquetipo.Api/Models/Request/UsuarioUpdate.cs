using System.ComponentModel.DataAnnotations;
using Arquetipo.Api.Models.Header;

namespace Arquetipo.Api.Models.Request;

public class UsuarioUpdate 
{
    [Required(ErrorMessage = "El id es obligatorio.")]
    public int? id { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string nombres { get; set; }
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo '{0}' debe ser un correo válido.")]
    public string mail { get; set; }
}