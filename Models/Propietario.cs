namespace ApiInmobiliaria.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Propietario
{
    [Key]
    public int IdPropietario { get; set; }
    [Required(ErrorMessage = "El DNI es requerido.")]
    public string Dni { get; set; } = "";

    [Required(ErrorMessage = "El apellido es requerido.")]
    public string Apellido { get; set; } = "";

    [Required(ErrorMessage = "El nombre es requerido.")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El telefono es requerido.")]
    public string Telefono { get; set; } ="";

    public bool Estado { get; set; }

    [Required(ErrorMessage = "El email es requerido.")]
    [EmailAddress(ErrorMessage = "Debe ingresar un email válido.")]
    public string? Email { get; set; } = "";

    [Required(ErrorMessage = "La clave es requerida.")]
    public string? Clave { get; set; } = "";

    public string? Avatar { get; set; }

    [NotMapped]
    public IFormFile? AvatarFile { get; set; }  // El archivo es opcional

    [JsonIgnore]
    public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>(); 
}
