

using Microsoft.EntityFrameworkCore;
using ApiInmobiliaria.Models;
//token
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiInmobiliaria.Services; 

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.UseUrls("http://localhost:5290","http://*:5290");

// Configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"])));

// Configuración de la autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => // La API web valida con token
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["TokenAuthentication:Issuer"], // Usar el issuer desde appsettings.json
            ValidAudience = configuration["TokenAuthentication:Audience"], // Usar el audience desde appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["TokenAuthentication:SecretKey"])), // Usar la clave desde appsettings.json
        };
    });

// Agregar servicios a la aplicación
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencia del servicio de envío de correos
builder.Services.AddScoped<EmailService>(); // Reemplaza "EmailService" con el nombre de tu clase de servicio de correo

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configuración del pipeline de manejo de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar archivos estáticos desde wwwroot
app.UseStaticFiles();  

// Habilitar HTTPS
//app.UseHttpsRedirection(); // Redirige todas las solicitudes HTTP a HTTPS

// Configurar CORS
app.UseCors("CorsPolicy");

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Mapea las rutas de los controladores

app.Run();
