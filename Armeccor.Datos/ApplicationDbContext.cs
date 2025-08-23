using Armeccor.Datos.Entidades;
using Armeccor.Datos.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Armeccor.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<AreaDetalleOrden> AreaDetalleOrdenes { get; set; }
        public DbSet<InsumoDetalleOrden> InsumoDetalleOrdenes { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación Orden - AreaDetalleOrden
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.AreaDetalleOrdenes)
                .WithOne(a => a.Orden)
                .HasForeignKey(a => a.OrdenId) // CORRECCIÓN
                .OnDelete(DeleteBehavior.Cascade);

            //Relacón Orden - InsumoDetalleOrden
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.InsumoOrdenes)
                .WithOne(i => i.Orden)
                .HasForeignKey(i => i.OrdenId) // CORRECCIÓN
                .OnDelete(DeleteBehavior.Cascade);

            // Relación AreaDetalleOrden - Area
            modelBuilder.Entity<AreaDetalleOrden>()
                .HasOne(ad => ad.Area)
                .WithMany(a => a.AreaOrdenes)
                .HasForeignKey(ad => ad.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación InsumoDetalleOrden - Insumo
            modelBuilder.Entity<InsumoDetalleOrden>()
                .HasOne(id => id.Insumo)
                .WithMany(i => i.InsumoOrdenes)
                .HasForeignKey(id => id.InsumoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Orden - Cliente
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId);

            // Relación Orden - Plano (nullable, sin cascada)
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Plano)
                .WithMany()
                .HasForeignKey(o => o.PlanoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación Orden - Entrega
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.Entregas)
                .WithOne(e => e.Orden)
                .HasForeignKey(e => e.OrdenId);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Orden)
                .WithMany(o => o.Entregas)
                .HasForeignKey(e => e.OrdenId);
        }
    }
}