using System.ComponentModel.DataAnnotations;

namespace ApiInmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        public int IdContrato { get; set; }
        public double Precio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaTerminacion { get; set; }
        
        // Foreign Key y Navegación
        public int IdInquilino { get; set; }
        public Inquilino Inquilino { get; set; }

        public int IdInmueble { get; set; }
        public Inmueble Inmueble { get; set; }

        public ICollection<Pago>? Pagos { get; set; }
    }
}
