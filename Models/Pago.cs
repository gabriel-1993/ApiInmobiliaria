using System.ComponentModel.DataAnnotations;

namespace ApiInmobiliaria.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }
        public int NroPago { get; set; }
        public DateTime Fecha { get; set; }
        public double Importe { get; set; }
        
        // Foreign Key y Navegación
        public int IdContrato { get; set; }
        public Contrato? Contrato { get; set; }
    }



}
