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
    [Authorize] // Asegura que solo usuarios autorizados accedan a los endpoints
    public class ContratosController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Constructor para inyectar el contexto de datos
        public ContratosController(AppDbContext context)
        {
            _context = context;
        }

 
        [HttpGet("contratoEnCursoInmuebleInquilinoPagos")]
        public async Task<IActionResult> contratoEnCursoInmuebleInquilinoDatos()
        {
            // Obtener el ID del propietario desde el token JWT
            var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

            // Verificar que el token sea válido
            if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioIdInt))
            {
                return Unauthorized("Token no contiene un ID válido.");
            }

            // Obtener los inmuebles del propietario y sus contratos, incluyendo pagos
            var inmueblesConContratosYPagos = await _context.Inmuebles
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
                        .Where(c => c.FechaTerminacion == null)
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
                            },
                            Pagos = c.Pagos // Agrega la lista de pagos del contrato sin incluir el contrato completo
                                .Select(p => new
                                {
                                    IdPago = p.IdPago,
                                    NroPago = p.NroPago,
                                    Fecha = p.Fecha,
                                    Importe = p.Importe
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            // Retornar la lista de inmuebles con contratos en curso y sus pagos
            return Ok(inmueblesConContratosYPagos);
        }





    }
}
