using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ApiInmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiInmobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Asegura que solo usuarios autorizados puedan acceder
    public class InquilinosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor que inyecta el contexto de datos
        public InquilinosController(AppDbContext context)
        {
            _context = context;
        }
     
      
        [HttpGet("contratosEnCursoInquilinos")]
        public async Task<IActionResult> contratosEnCursoInquilinos()
        {
            // Obtener el ID del propietario desde el token JWT
            var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

            // Verificar que el token sea válido
            if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioIdInt))
            {
                return Unauthorized("Token no contiene un ID válido.");
            }

            // Obtener los inmuebles del propietario y sus contratos, incluyendo pagos
            var inmuebleContratoInquilinoEnCurso = await _context.Inmuebles
                .Where(i => i.IdPropietario == propietarioIdInt &&
                            i.Contratos.Any(c => c.FechaTerminacion == null)) // Filtra solo contratos en curso
                .Select(i => new
                {
                    IdInmueble = i.IdInmueble,
                    Direccion = i.Direccion,
                    Ambientes = i.Ambientes,
                    Tipo = i.Tipo,
                    Uso = i.Uso,
                    Precio = i.Precio,
                    Avatar = i.Avatar,
                    IdPropietario = i.IdPropietario,
                    Contratos = i.Contratos
                        .Where(c => c.FechaTerminacion == null) // Filtra contratos en curso
                        .Select(c => new
                        {
                            IdContrato = c.IdContrato,
                            PrecioContrato = c.Precio,
                            FechaInicio = c.FechaInicio,
                            FechaFin = c.FechaFin,
                            FechaTerminacion = c.FechaTerminacion,
                            Inquilino = new
                            {
                                IdInquilino = c.Inquilino.IdInquilino,
                                Dni = c.Inquilino.Dni,
                                Apellido = c.Inquilino.Apellido,
                                Nombre = c.Inquilino.Nombre,
                                Telefono = c.Inquilino.Telefono,
                                NombreGarante = c.Inquilino.NombreGarante,
                                ApellidoGarante = c.Inquilino.ApellidoGarante,
                                TelefonoGarante = c.Inquilino.TelefonoGarante
                            }
                        })
                        .ToList()
                })
                .ToListAsync();

            // Retornar la lista de inmuebles con contratos en curso y sus pagos
            return Ok(inmuebleContratoInquilinoEnCurso);
        }




    }
}
