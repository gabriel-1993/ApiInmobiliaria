

namespace ApiInmobiliaria.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Inmueble> Inmuebles { get; set; } // Agregar la tabla de Inmuebles

    public DbSet<Contrato> Contratos { get; set; }

    public DbSet<Inquilino> Inquilinos { get; set; }
    
    public DbSet<Pago> Pagos { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configuración de Inmueble
    modelBuilder.Entity<Inmueble>()
        .HasKey(i => i.IdInmueble);

    modelBuilder.Entity<Inmueble>()
        .HasOne(i => i.Propietario)
        .WithMany(p => p.Inmuebles)
        .HasForeignKey(i => i.IdPropietario);

    // Configuración de Contrato
    modelBuilder.Entity<Contrato>()
        .HasKey(c => c.IdContrato);

    modelBuilder.Entity<Contrato>()
        .HasOne(c => c.Inquilino)
        .WithMany(i => i.Contratos)
        .HasForeignKey(c => c.IdInquilino);

    modelBuilder.Entity<Contrato>()
        .HasOne(c => c.Inmueble)
        .WithMany(i => i.Contratos)
        .HasForeignKey(c => c.IdInmueble);

    // Configuración de Pago
    modelBuilder.Entity<Pago>()
        .HasKey(p => p.IdPago);

    modelBuilder.Entity<Pago>()
        .HasOne(p => p.Contrato)
        .WithMany(c => c.Pagos)
        .HasForeignKey(p => p.IdContrato);
}

}

