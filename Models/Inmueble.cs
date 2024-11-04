

namespace ApiInmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inmueble
{
    [Key]
    public int IdInmueble { get; set; } // Auto increment

    [Required(ErrorMessage = "La dirección es requerida.")]
    public string Direccion { get; set; } = string.Empty; // Required

    [Required(ErrorMessage = "El número de ambientes es requerido.")]
    public int Ambientes { get; set; } // Required

    [Required(ErrorMessage = "El tipo de inmueble es requerido.")]
    public string Tipo { get; set; } = string.Empty; // Required

    [Required(ErrorMessage = "El uso del inmueble es requerido.")]
    public string Uso { get; set; } = string.Empty; // Required

    [Required(ErrorMessage = "El precio es requerido.")]
    public double Precio { get; set; } // Required

    public bool? Disponible { get; set; } = false; //la base de datos por defecto esta en false por si llega vacio
    public string? Avatar { get; set; } 

    // Foreign Key to Propietario
    [ForeignKey("Propietario")]
    public int IdPropietario { get; set; }
    
    public Propietario? Propietario { get; set; } = null!; // Navigation Property

    public ICollection<Contrato>? Contratos { get; set; }

}
