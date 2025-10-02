using Armeccor.Datos.Entidades;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<MedioDePago> MedioDePagos { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<EventoNotificado> EventosNotificados { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {      
            // ===========================
            // Relación Orden - AreaDetalleOrden
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.AreaDetalleOrdenes)
                .WithOne(a => a.Orden)
                .HasForeignKey(a => a.OrdenId)
                .OnDelete(DeleteBehavior.Cascade); // Ahora si borras la orden, OrdenId se pone null

            // ===========================
            // Relación Orden - InsumoDetalleOrden
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.InsumoOrdenes)
                .WithOne(i => i.Orden)
                .HasForeignKey(i => i.OrdenId)
                .OnDelete(DeleteBehavior.Cascade); // SetNull para no exigir cascade

            // ===========================
            // Relación AreaDetalleOrden - Area
            // ===========================
            modelBuilder.Entity<AreaDetalleOrden>()
                .HasOne(ad => ad.Area)
                .WithMany(a => a.AreaOrdenes)
                .HasForeignKey(ad => ad.AreaId)
                .OnDelete(DeleteBehavior.SetNull); // AreaId se pone null si borran el Area

            // ===========================
            // Relación InsumoDetalleOrden - Insumo
            // ===========================
            modelBuilder.Entity<InsumoDetalleOrden>()
                .HasOne(id => id.Insumo)
                .WithMany(i => i.InsumoOrdenes)
                .HasForeignKey(id => id.InsumoId)
                .OnDelete(DeleteBehavior.Cascade); // Similar, se pone null

            // ===========================
            // Relación Orden - Cliente
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.Cascade); // ClienteId se pone null si borran cliente

            // ===========================
            // Relación Orden - Plano (nullable, sin cascada)
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Plano)
                .WithMany()
                .HasForeignKey(o => o.PlanoId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===========================
            // Relación Orden - Entrega
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.Entregas)
                .WithOne(e => e.Orden)
                .HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Orden)
                .WithMany(o => o.Entregas)
                .HasForeignKey(e => e.OrdenId);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Medio_De_Pago)
                .WithMany(m => m.Entregas)   // un medio de pago → muchas entregas
                .HasForeignKey(e => e.MedioDePagoId)
                .OnDelete(DeleteBehavior.Cascade); // o Cascade, según lo que prefieras

            //Configuración de Notificaciones

            // Relación con Insumo
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Insumo)
                .WithMany(i => i.Notificaciones)
                .HasForeignKey(n => n.InsumoId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada

            // Relación con Orden
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Orden)
                .WithMany(o => o.Notificaciones)
                .HasForeignKey(n => n.OrdenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con AreaDetalleOrden
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.AreaDetalle)
                .WithMany(a => a.Notificaciones)
                .HasForeignKey(n => n.AreaDetalleId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            var areas = new List<Area>
            {
                new Area()
                {
                    Id = 1,
                    NombreArea = "Mecanizado"
                },
                new Area()
                {
                    Id = 2,
                    NombreArea = "Soldadura"
                },
                new Area()
                {
                    Id = 3,
                    NombreArea = "Pintura"
                },
                new Area()
                {
                    Id = 4,
                    NombreArea = "Calidad"
                },
                new Area()
                {
                    Id = 5,
                    NombreArea = "Embalaje"
                },
                new Area()
                {
                    Id = 6,
                    NombreArea = "Logística"
                },
                new Area()
                {
                    Id = 7,
                    NombreArea = "Montaje"
                },
                new Area()
                {
                    Id = 8,
                    NombreArea = "Mantenimiento"
                },
                new Area()
                {
                    Id = 9,
                    NombreArea = "Administración"
                },
                new Area()
                {
                    Id = 10,
                    NombreArea = "Compras"
                }
            };

            modelBuilder.Entity<Area>().HasData(areas);

            var medios = new List<MedioDePago>
            {
                new MedioDePago()
                {
                    Id = 1,
                    Nombre_Medio = "Efectivo"
                },
                new MedioDePago()
                {
                    Id = 2,
                    Nombre_Medio = "Tarjeta de crédito"
                },
                new MedioDePago()
                {
                    Id = 3,
                    Nombre_Medio = "Transferencia bancaria"
                },
                new MedioDePago()
                {
                    Id = 4,
                    Nombre_Medio = "Cheque"
                },
                new MedioDePago()
                {
                    Id = 5,
                    Nombre_Medio = "Mercado Pago"
                },

                new MedioDePago()
                {
                    Id = 6,
                    Nombre_Medio = "PayPal"
                }
            };

            modelBuilder.Entity<MedioDePago>().HasData(medios);
        }
    }
}