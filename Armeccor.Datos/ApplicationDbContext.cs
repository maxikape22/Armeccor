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
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalleInsumo> PedidoDetalleInsumos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<UnidadConversion> UnidadConversiones { get; set; }
        public DbSet<Estante> Estantes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

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
                .OnDelete(DeleteBehavior.SetNull); // SetNull para no exigir cascade

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
                .OnDelete(DeleteBehavior.SetNull); // Similar, se pone null

            // ===========================
            // Relación Orden - Cliente
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId)
                .OnDelete(DeleteBehavior.SetNull); // ClienteId se pone null si borran cliente

            // ===========================
            // Relación Plano - Orden (nullable, sin cascada)
            // ===========================
            modelBuilder.Entity<Plano>()
                .HasOne(p => p.Orden)
                .WithMany(o => o.Planos)
                .HasForeignKey(p => p.OrdenId)
                .OnDelete(DeleteBehavior.SetNull);


            // ===========================
            // Relación Orden - Entrega
            // ===========================
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.Entregas)
                .WithOne(e => e.Orden)
                .HasForeignKey(e => e.OrdenId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Orden)
                .WithMany(o => o.Entregas)
                .HasForeignKey(e => e.OrdenId);

            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Medio_De_Pago)
                .WithMany(m => m.Entregas)   // un medio de pago → muchas entregas
                .HasForeignKey(e => e.MedioDePagoId)
                .OnDelete(DeleteBehavior.SetNull); 

            //Configuraciones para DetallePedido - Pedido - Insumo

            modelBuilder.Entity<PedidoDetalleInsumo>()
               .HasOne(ad => ad.Pedido)
               .WithMany(a => a.DetallePedidos)
               .HasForeignKey(ad => ad.IdPedido)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PedidoDetalleInsumo>()
                .HasOne(pd => pd.Insumo)
                .WithMany(i => i.DetallePedidos)
                .HasForeignKey(pd => pd.IdInsumo)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pedido>()       
                .HasOne(p => p.Proveedor)  
                .WithMany(pv => pv.Pedidos)     
                .HasForeignKey(p => p.IdProveedor)     
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false); // 👈 permite NULL

            //Configuracion de Estante - Insumo

            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.Estante)
                .WithMany(e => e.Insumos)
                .HasForeignKey(i => i.EstanteId)
                .OnDelete(DeleteBehavior.SetNull);

            //Configuracion eliminacion fecha de baja para todas las entidades para la eliminación logica

            modelBuilder
                .Entity<Estante>()
                .Property(e => e.FechaBaja)
                .HasColumnType("datetime2(7)");

            modelBuilder
                .Entity<PedidoDetalleInsumo>()
                .Property(d=>d.FechaBaja)
                .HasColumnType("datetime2(7)");

            modelBuilder
                .Entity<Insumo>()
                .Property(i => i.FechaBorrado)
                .HasColumnType("datetime2(7)");

            modelBuilder
                .Entity<InsumoDetalleOrden>()
                .Property(d=>d.FechaBaja)
                .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<Cliente>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<Area>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<AreaDetalleOrden>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<Orden>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<Entrega>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

            modelBuilder
               .Entity<Plano>()
               .Property(d => d.FechaBaja)
               .HasColumnType("datetime2(7)");

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

            var proveedores = new List<Proveedor>
            {
                new Proveedor()
                {
                    Id = 1,
                    Nombre = "Acme S.A.",
                    Correo = "JuanPérez322@gmail.com",
                    Telefono = "123456789",
                },

                new Proveedor()
                {
                    Id = 2,
                    Nombre = "Industrias Globales",
                    Correo = "MaríaGómez323@gmail.com",
                    Telefono = "987654321",
                },
                new Proveedor()
                {
                    Id = 3,
                    Nombre = "Suministros Técnicos",
                    Correo = "CarlosRodríguez243@gmail.com",
                    Telefono = "456123789",
                },
                new Proveedor()
                {
                    Id = 4,
                    Nombre = "Materiales y Más",
                    Correo = "AnaFernández423@gmail.com",
                    Telefono = "789456123",
                },
                new Proveedor()
                {
                    Id = 5,
                    Nombre = "Soluciones Industriales",
                    Correo = "LuisMartínez902@gmail.com",
                    Telefono = "321654987",
                },
                new Proveedor()
                {
                    Id = 6,
                    Nombre = "Equipos y Herramientas S.R.L.",
                    Correo = "SofíaLópez434@gmail.com", 
                    Telefono = "654987321",
                },
            };

            modelBuilder.Entity<Proveedor>().HasData(proveedores);
        }
    }
}