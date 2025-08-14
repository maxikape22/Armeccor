using Armeccor.Datos.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Armeccor.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaOrden> AreaOrdenes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Entrega> Entregas { get; set; }
        public DbSet<Insumo> Insumos { get; set; }
        public DbSet<InsumoOrden> InsumoOrdenes { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Pieza> Piezas { get; set; }

        #region
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // ... otras configuraciones

        //    modelBuilder.Entity<AreaOrden>()
        //        .HasOne(ao => ao.Orden)
        //        .WithMany(o => o.AreaOrdenes)
        //        .HasForeignKey(ao => ao.IdOrden)
        //        .OnDelete(DeleteBehavior.NoAction); // <-- Esto deshabilita el borrado en cascada

        //    // Nota: El borrado en cascada para la relación con 'Areas' podría estar bien,
        //    // pero debes revisar tu modelo completo. El error apunta específicamente a 'IdOrden'.
        //    modelBuilder.Entity<AreaOrden>()
        //        .HasOne(ao => ao.Area)
        //        .WithMany(a => a.AreasOrden)
        //        .HasForeignKey(ao => ao.IdArea)
        //        .OnDelete(DeleteBehavior.NoAction); // <-- Si la relación con 'Areas' también causa un ciclo, configúrala aquí.


        //    modelBuilder
        //        .Entity<AreaOrden>()
        //        .HasOne(ao => ao.Orden)
        //        .WithMany(a => a.AreaOrdenes)
        //        .HasForeignKey(ao => ao.IdOrden)
        //        .OnDelete(DeleteBehavior.NoAction);
        //}

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la relación entre Orden y AreaOrden
            modelBuilder.Entity<AreaOrden>()
                .HasOne(ao => ao.Orden)
                .WithMany(o => o.AreaOrdenes)
                .HasForeignKey(ao => ao.IdOrden)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración para la relación entre Orden y Pieza
            modelBuilder.Entity<Pieza>()
                .HasOne(p => p.Orden)
                .WithMany(o => o.Piezas)
                .HasForeignKey(p => p.IdOrden)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración para la relación entre Orden y Entrega
            modelBuilder.Entity<Entrega>()
                .HasOne(e => e.Orden)
                .WithMany(o => o.Entregas)
                .HasForeignKey(e => e.IdOrden)
                .OnDelete(DeleteBehavior.NoAction);

            // Configuración para la relación entre Orden y InsumoOrden
            modelBuilder.Entity<InsumoOrden>()
                .HasOne(io => io.Orden)
                .WithMany(o => o.InsumosOrden)
                .HasForeignKey(io => io.IdOrden)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
