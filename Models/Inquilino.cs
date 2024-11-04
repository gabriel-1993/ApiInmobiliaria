using System.ComponentModel.DataAnnotations;

namespace ApiInmobiliaria.Models
{
    public class Inquilino
    {
        [Key]
        public int IdInquilino { get; set; }
        public int Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string NombreGarante { get; set; }
        public string ApellidoGarante { get; set; }
        public string TelefonoGarante { get; set; }
        
        public ICollection<Contrato>? Contratos { get; set; }
    }
}
