namespace ApiInmobiliaria.Models.Dtos; 
using System.ComponentModel.DataAnnotations;

// MODEL PARA MODIFICAR UNICAMENTE DE UN PROPIETARIO SU DNI APELLIDO NOMBRE TELEFONO  EMAIL
// Para evitar errores con los demas atributos como clave o avatar, esos se modifican individualmente cada uno


    public class PropietarioUpdateDto
{
    [Required(ErrorMessage = "El DNI es requerido.")]
    public string Dni { get; set; } = "";

    [Required(ErrorMessage = "El apellido es requerido.")]
    public string Apellido { get; set; } = "";

    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El telefono es requerido.")]
    public string Telefono { get; set; } = "";

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un email válido.")]

    public string? Email { get; set; }
}


