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
        public DbSet<InsumoOrden> InsumosOrden { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<Plano> Planos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            /* NOTA IMPORTANTE: HE ACTIVADO LA ELIMINACION EN CASCADA PARA TODAS LAS RELACIONES
             * PORQUE LA TABLA ORIGINAL DE AREA TRAE EL ORIGEN DE LA RELACION CON ORDEN Y SUS 
             * ASOCIACIONES HIJAS("CLIENTE" Y "ENTREGA") CON EL PADRE "ORDEN". ESTO NO ES PARA 
             * CONVENIENTE, ASÍ QUE POR EL MOMENTO ES UNA SOLUCIÓN TEMPORAL. 
             */


            // Relación Orden - Area
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Area)
                .WithMany(a => a.Ordenes)
                .HasForeignKey(o => o.AreaId)
            .OnDelete(DeleteBehavior.SetNull);

            // Relación Orden - Cliente
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Cliente)
                .WithMany(c => c.Ordenes)
                .HasForeignKey(o => o.ClienteId);
              //  .OnDelete(DeleteBehavior.Cascade);

            // Relación Orden - Plano (nullable, sin cascada)
            modelBuilder.Entity<Orden>()
                .HasOne(o => o.Plano)
                .WithMany() // Plano no tiene lista de órdenes
                .HasForeignKey(o => o.PlanoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación Orden - Entrega
            modelBuilder.Entity<Orden>()
                .HasMany(o => o.Entregas)
                .WithOne(e => e.Orden)
                .HasForeignKey(e => e.OrdenId);
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entrega>()
        .HasOne(e => e.Orden)
        .WithMany(o => o.Entregas) // si en Orden pusiste ICollection<Entrega>
        .HasForeignKey(e => e.OrdenId);
        //.OnDelete(DeleteBehavior.Cascade);

            // Relación N:N entre Orden e Insumo usando InsumoOrden
            modelBuilder.Entity<InsumoOrden>()
                .HasKey(io => new { io.OrdenId, io.InsumoId });

            modelBuilder.Entity<InsumoOrden>()
                .HasOne(io => io.Orden)
                .WithMany(o => o.InsumosOrden)
                .HasForeignKey(io => io.OrdenId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InsumoOrden>()
                .HasOne(io => io.Insumo)
                .WithMany(i => i.InsumosOrden)
                .HasForeignKey(io => io.InsumoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
