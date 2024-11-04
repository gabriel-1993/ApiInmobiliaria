

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiInmobiliaria.Models;
using ApiInmobiliaria.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // Asegúrate de que esto esté aquí
using ApiInmobiliaria.Services;


namespace ApiInmobiliaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropietariosController : ControllerBase
    {
        // ATRIBUTOS
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Propietario> _passwordHasher;

        private readonly IWebHostEnvironment environment;

        private readonly EmailService _emailService;


        // CONSTRUCTOR
        public PropietariosController(AppDbContext context, IConfiguration configuration, IWebHostEnvironment env, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Propietario>();
            environment = env;
            _emailService = emailService;

        }

        // POST: api/Propietarios
        [HttpPost]
        public async Task<ActionResult<Propietario>> PostPropietario([FromBody] Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                // Hash de la clave utilizando el mismo salt
                if (!string.IsNullOrEmpty(propietario.Clave))
                {
                    var salt = Encoding.ASCII.GetBytes(_configuration["Salt"]); // Obtener el salt de la configuración
                    propietario.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                }

                _context.Propietarios.Add(propietario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPropietario), new { id = propietario.IdPropietario }, propietario);
            }

            return BadRequest(ModelState);
        }

        // GET: api/Propietarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Propietario>> GetPropietario(int id)
        {
            var propietario = await _context.Propietarios.FindAsync(id);

            if (propietario == null)
            {
                return NotFound();
            }

            return Ok(propietario);
        }


        // Método para iniciar sesión
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                // Hashear la clave ingresada
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: Encoding.ASCII.GetBytes(_configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                // Buscar al propietario por email
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Email);

                // Validar el propietario y la clave
                if (propietario == null || propietario.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }

                // Generar el token JWT
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenAuthentication:SecretKey"]));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, propietario.Email),
                    new Claim("FullName", $"{propietario.Nombre} {propietario.Apellido}"),
                    new Claim(ClaimTypes.Role, "Propietario"),
                    new Claim("PropietarioId", propietario.IdPropietario.ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["TokenAuthentication:Issuer"],
                    audience: _configuration["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credenciales
                );

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("perfil")]
        [Authorize]
        public async Task<ActionResult<Propietario>> GetPerfilPropietario()
        {
            try
            {
                // Obtener el ID del propietario desde el token JWT
                var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

                if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioId))
                {
                    return Unauthorized("Token no contiene un ID válido");
                }

                // Buscar al propietario por su ID
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(p => p.IdPropietario == int.Parse(propietarioIdClaim));

                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado");
                }

                return Ok(propietario);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al obtener el perfil: " + ex.Message);
            }
        }



        [HttpPut("modificarPerfil")]
        [Authorize]
        //El model de este endpoint se encuentra en Models-->Dtos-->PropietarioUpdateDto.cs
        public async Task<IActionResult> ModificarPerfil([FromForm] PropietarioUpdateDto updatedPropietario)
        {
            try
            {
                // Obtener el ID del propietario desde el token JWT
                var propietarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "PropietarioId")?.Value;

                if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioId))
                {
                    return Unauthorized("Token no contiene un ID válido");
                }

                // Buscar al propietario por su ID
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(p => p.IdPropietario == propietarioId);

                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado");
                }

                // Actualizar los datos con los valores recibidos en el DTO
                propietario.Dni = updatedPropietario.Dni;
                propietario.Nombre = updatedPropietario.Nombre;
                propietario.Apellido = updatedPropietario.Apellido;
                propietario.Telefono = updatedPropietario.Telefono;
                propietario.Email = updatedPropietario.Email;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Perfil actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar el perfil: " + ex.Message);
            }
        }



        [HttpPut("cambiarContraseña")]
        [Authorize]
        public async Task<IActionResult> CambiarContraseña([FromForm] CambiarContraseñaView model)
        {
            try
            {
                // Obtener el ID del propietario desde el token JWT
                var propietarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "PropietarioId")?.Value;

                if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int propietarioId))
                {
                    return Unauthorized("Token no contiene un ID válido");
                }

                // Buscar al propietario por su ID
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(p => p.IdPropietario == propietarioId);

                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado");
                }

                // Verificar si las contraseñas nuevas coinciden
                if (model.NuevaContraseña != model.RepetirContraseña)
                {
                    return BadRequest("Las nuevas contraseñas no coinciden.");
                }

                // Hashear la clave ingresada
                string hashedCurrentPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.ContraseñaActual,
                    salt: Encoding.ASCII.GetBytes(_configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                // Verificar si la contraseña actual es correcta
                if (propietario.Clave != hashedCurrentPassword)
                {
                    return BadRequest("La contraseña actual es incorrecta.");
                }

                // Hashear la nueva clave
                string hashedNewPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: model.NuevaContraseña,
                    salt: Encoding.ASCII.GetBytes(_configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                // Actualizar la contraseña
                propietario.Clave = hashedNewPassword;
                await _context.SaveChangesAsync();

                return Ok("Contraseña cambiada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al cambiar la contraseña: " + ex.Message);
            }
        }




        [HttpPut("modificarAvatar")]
        [Authorize]
        public async Task<IActionResult> ModificarAvatar([FromForm] IFormFile avatarFile)
        {
            try
            {
                // Obtener el ID del propietario desde el token JWT
                var propietarioIdClaim = User.Claims.FirstOrDefault(c => c.Type == "PropietarioId")?.Value;

                if (string.IsNullOrEmpty(propietarioIdClaim) || !int.TryParse(propietarioIdClaim, out int IdPropietario))
                {
                    return Unauthorized("Token no contiene un ID válido");
                }

                // Buscar al propietario por su ID
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(p => p.IdPropietario == IdPropietario);
                if (propietario == null)
                {
                    return NotFound("Propietario no encontrado");
                }

                // Validar que se ha recibido un archivo
                if (avatarFile == null || avatarFile.Length == 0)
                {
                    return BadRequest("No se ha recibido un archivo de avatar.");
                }

                // Definir la ruta donde se guardará el avatar
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Guardar la ruta del avatar anterior
                var avatarAnterior = propietario.Avatar;

                // Crear un nombre para el archivo utilizando el ID, nombre completo, y un GUID para asegurarte de que sea único
                var fullName = $"{propietario.Nombre}_{propietario.Apellido}";
                var uniqueId = Guid.NewGuid().ToString();  // Generar un identificador único
                var fileName = $"{IdPropietario}_{fullName}_{uniqueId}{Path.GetExtension(avatarFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Guardar el nuevo archivo en el sistema
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                // Actualizar la propiedad del avatar en el modelo
                propietario.Avatar = $"/avatars/{fileName}"; // Asumiendo que almacenarás solo la URL relativa

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                // Eliminar el avatar anterior si existe
                if (!string.IsNullOrEmpty(avatarAnterior))
                {
                    var avatarAnteriorPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", avatarAnterior.TrimStart('/'));

                    if (System.IO.File.Exists(avatarAnteriorPath))
                    {
                        System.IO.File.Delete(avatarAnteriorPath);  // Eliminar el archivo anterior
                    }
                }

                return Ok("Avatar actualizado correctamente.");
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





        // // Método para generar una clave aleatoria
        public string GenerarClaveAleatoria(int longitud)
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();

            return new string(Enumerable.Repeat(letras, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }




        [HttpPost("restablecerClave")]
        [AllowAnonymous]
        public async Task<IActionResult> RestablecerClave([FromForm] string email)
        {
            try
            {
                // Buscar al propietario por email
                var propietario = await _context.Propietarios.FirstOrDefaultAsync(x => x.Email == email);

                if (propietario == null)
                {
                    return BadRequest("El correo electrónico no está registrado.");
                }

                // Generar el token JWT para restablecimiento de contraseña
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenAuthentication:SecretKey"]));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, propietario.Email),
            new Claim("PropietarioId", propietario.IdPropietario.ToString()),
            new Claim("Purpose", "PasswordReset") // Propósito del token
        };



                var token = new JwtSecurityToken(
                    issuer: _configuration["TokenAuthentication:Issuer"],
                    audience: _configuration["TokenAuthentication:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5), // Token válido por 5 minutos
                    signingCredentials: credenciales
                );

                // Obtener el dominio o la IP
                var dominio = environment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.misitio.com";

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Contenido del correo de restablecimiento
                var mensajeHtml = $@"
            <p>Si usted no solicitó restablecer su contraseña en Inmobiliaria La Punta, ignore este correo. Sino, haga clic en el siguiente enlace: se va restablecer su contraseña y podrá ingresar con la nueva contraseña para modificarla si lo desea.<strong> Va a recibir un nuevo email con su nueva contraseña</strong>.  </p>
            <form action='http://{dominio}:5290/api/propietarios/confirmarRestablecerClave' method='POST'>
                <input type='hidden' name='token' value='{tokenString}'>
                <button type='submit'>Restablecer contraseña</button>
            </form>";

                // Enviar el correo
                await _emailService.EnviarCorreoAsync(email, "Restablecer contraseña", mensajeHtml);

                return Ok("Correo de restablecimiento de contraseña enviado.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al generar el token o enviar el correo: {ex.Message}");
            }
        }


        [HttpPost("confirmarRestablecerClave")]
        [AllowAnonymous]
        public async Task<IActionResult> confirmarRestablecerClave([FromForm] string token)
        {
            try
            {
                // Validar el token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["TokenAuthentication:SecretKey"]);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration["TokenAuthentication:Issuer"],
                    ValidAudience = _configuration["TokenAuthentication:Audience"]
                };

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Extraer el Id del propietario del token
                var idClaim = principal.FindFirst("PropietarioId");
                if (idClaim == null)
                {
                    return BadRequest("Token no válido.");
                }

                // Verificar el formato del ID como un int
                if (!int.TryParse(idClaim.Value, out int propietarioId))
                {
                    return BadRequest("Formato de ID no válido.");
                }

                // Buscar al propietario por Id
                var propietario = await _context.Propietarios.FindAsync(propietarioId);
                if (propietario == null)
                {
                    return BadRequest("El propietario no existe.");
                }

                // Generar una nueva clave aleatoria de 4 letras
                string nuevaClave = GenerarClaveAleatoria(4);

                // Hashear la nueva clave usando la misma lógica que en el método de login
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: nuevaClave,
                    salt: Encoding.ASCII.GetBytes(_configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                // Actualizar la clave en la base de datos
                propietario.Clave = hashed;
                _context.Propietarios.Update(propietario);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error al guardar la nueva contraseña: {ex.Message}");
                }

                // Contenido del correo de confirmación de restablecimiento
                var mensajeHtml = $"<p>Su contraseña ha sido restablecida. La nueva contraseña es: <strong>{nuevaClave}</strong>. Puede <strong>Iniciar sesion</strong> y <strong>Modificarla</strong> desde su <strong>Menu -->Perfil -->Modificar Clave</strong>...</p>";

                // Enviar el correo
                await _emailService.EnviarCorreoAsync(propietario.Email, "Contraseña restablecida", mensajeHtml);

                return Ok("Contraseña restablecida con éxito. Se ha enviado un correo con la nueva contraseña.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al restablecer la contraseña: {ex.Message}");
            }
        }





    }







    // CLASES AUXILIARES para Login y para CambiarPass 
    //CARPETA MODELS--> Dtos-->  Model que se utiliza solo para modificar esos datos de propietario
    public class LoginView
    {
        public string Email { get; set; }
        public string Clave { get; set; }
    }

    public class CambiarContraseñaView
    {
        required
        public string ContraseñaActual
        { get; set; }
        required
        public string NuevaContraseña
        { get; set; }
        required

public string RepetirContraseña
        { get; set; }
    }



}