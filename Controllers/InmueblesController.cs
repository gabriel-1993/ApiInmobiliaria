using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInmobiliaria.Models; // Asegúrate de que esté correctamente referenciado
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ApiInmobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InmueblesController : ControllerBase
    {


        // ATRIBUTOS
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        // CONSTRUCTOR
        public InmueblesController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("misInmuebles")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetMisInmuebles()
        {
            // Obtener el ID del propietario del token
            var propietarioIdClaim = User.FindFirst("PropietarioId");

            if (propietarioIdClaim == null)
            {
                return Unauthorized(); // Retorna 401 si no se encuentra el ID
            }

            // Convertir el ID a un entero
            if (!int.TryParse(propietarioIdClaim.Value, out int id))
            {
                return BadRequest("ID de propietario no válido."); // Retorna 400 si el ID no es válido
            }

            // Obtener los inmuebles del propietario, incluyendo el propietario
            var inmuebles = await _context.Inmuebles
                                           .Include(i => i.Propietario) // Incluir la entidad Propietario
                                           .Where(i => i.IdPropietario == id)
                                           .ToListAsync();

            // Retornar la lista de inmuebles
            return Ok(inmuebles);
        }




        // GET: api/inmuebles/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return inmueble;
        }




        // POST: api/inmuebles
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<Inmueble>> CreateInmueble(Inmueble inmueble)
        {
            _context.Inmuebles.Add(inmueble);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInmueble), new { id = inmueble.IdInmueble }, inmueble);
        }



        [Authorize]
        [HttpPut("publicarOnOff")]
        public async Task<IActionResult> publicarOnOff([FromForm] PublicarOnOffRequest request)
        {
            // Obtener el ID del propietario desde el token JWT
            var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

            // Verificar que el propietarioId no sea nulo o vacío
            if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioIdInt))
            {
                return Unauthorized("Token no contiene un ID válido.");
            }

            // Buscar el inmueble y verificar si pertenece al propietario
            var inmueble = await _context.Inmuebles
                .FirstOrDefaultAsync(i => i.IdInmueble == request.IdInmueble && i.IdPropietario == propietarioIdInt);

            if (inmueble == null)
            {
                return NotFound("Inmueble no encontrado o no pertenece al propietario.");
            }

            // Cambiar el valor de 'Disponible' al valor opuesto
            inmueble.Disponible = !inmueble.Disponible;
            _context.Entry(inmueble).Property(i => i.Disponible).IsModified = true;

            await _context.SaveChangesAsync();

            return Ok("Disponibilidad actualizada con éxito.");
        }




        [HttpPost("agregarInmueble")]
        public async Task<IActionResult> AgregarInmueble([FromForm] Inmueble inmueble)
        {
            // Obtener el ID del propietario desde el token JWT
            var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

            // Verificar que el token sea válido
            if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioIdInt))
            {
                return Unauthorized("Token no contiene un ID válido.");
            }

            // Verificar que el propietario existe en la base de datos
            var propietario = await _context.Propietarios.FindAsync(propietarioIdInt);
            if (propietario == null)
            {
                return NotFound("El propietario asociado al token no existe.");
            }

            // Asignar el IdPropietario del token al inmueble
            inmueble.IdPropietario = propietarioIdInt;

            // Validar que el modelo sea válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Agregar inmueble a la base de datos
            _context.Inmuebles.Add(inmueble);
            await _context.SaveChangesAsync();

            // Retornar el ID del inmueble agregado
            return Ok(new
            {
                message = "Inmueble agregado exitosamente.",
                inmuebleId = inmueble.IdInmueble.ToString()
            });
        }



        [HttpPut("modificarAvatar")]
        [Authorize]
        public async Task<IActionResult> ModificarAvatar([FromForm] int idInmueble, [FromForm] IFormFile avatarFile)
        {
            try
            {
                // Obtener el ID del propietario desde el token JWT
                var propietarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "PropietarioId")?.Value;

                if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int IdPropietario))
                {
                    return Unauthorized("Token no contiene un ID válido");
                }

                // Buscar el inmueble por su ID y verificar que pertenezca al propietario
                var inmueble = await _context.Inmuebles.FirstOrDefaultAsync(i => i.IdInmueble == idInmueble && i.IdPropietario == IdPropietario);
                if (inmueble == null)
                {
                    return NotFound("Inmueble no encontrado o no pertenece al propietario");
                }

                // Validar que se ha recibido un archivo
                if (avatarFile == null || avatarFile.Length == 0)
                {
                    return BadRequest("No se ha recibido un archivo de avatar.");
                }

                // Definir la ruta donde se guardará el avatar
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imgInmuebles");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Guardar la ruta del avatar anterior
                var avatarAnterior = inmueble.Avatar;

                // Crear un nombre único para el archivo usando ID y un GUID
                var uniqueId = Guid.NewGuid().ToString();
                var fileName = $"{idInmueble}_{uniqueId}{Path.GetExtension(avatarFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Guardar el nuevo archivo en el sistema
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // Actualizar la propiedad del avatar en el modelo
                inmueble.Avatar = $"/imgInmuebles/{fileName}";

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Eliminar el avatar anterior si existe
                if (!string.IsNullOrEmpty(avatarAnterior))
                {
                    var avatarAnteriorPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", avatarAnterior.TrimStart('/'));
                    if (System.IO.File.Exists(avatarAnteriorPath))
                    {
                        System.IO.File.Delete(avatarAnteriorPath);
                    }
                }

                return Ok("Avatar del inmueble actualizado correctamente.");
            }
            catch (UnauthorizedAccessException uex)
            {
                return BadRequest("Error de permisos: " + uex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el avatar: " + ex.Message);
            }
        }


    

        [HttpGet("inmueblesConContratoEnCurso")]
        public async Task<IActionResult> InmueblesConContratoEnCurso()
        {
            var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

            if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioIdInt))
            {
                return Unauthorized("Token no contiene un ID válido.");
            }

            var inmuebles = await _context.Inmuebles
                .Where(i => i.IdPropietario == propietarioIdInt &&
                            i.Contratos.Any(c => c.FechaTerminacion == null))
                .Select(i => new
                {
                    IdInmueble = i.IdInmueble,
                    Direccion = i.Direccion,
                    Ambientes = i.Ambientes,
                    Tipo = i.Tipo,
                    Uso = i.Uso,
                    Precio = i.Precio,
                    Avatar = i.Avatar
                })
                .ToListAsync();

            return Ok(inmuebles);
        }



    }


    public class PublicarOnOffRequest
    {
        public int IdInmueble { get; set; }
    }


}