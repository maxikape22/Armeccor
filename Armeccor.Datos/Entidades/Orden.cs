using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    [Index(nameof(NroOT), Name = "NroOT_UQ", IsUnique = true)]
    public class Orden
    {
        public int Id { get; set; }
        [Required]
        public int NroOT { get; set; }
        [Required]
        public string NombreOrden { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required, MaxLength(50)]
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaPactada { get; set; } = DateTime.Now;
        public DateTime FechaEntrega { get; set; } = DateTime.Now;
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public int? PlanoId { get; set; } // puede ser null si no se cargó aún
        public Plano Plano { get; set; }
        public ICollection<Entrega> Entregas { get; set; } 
        public ICollection<AreaDetalleOrden> AreaDetalleOrdenes { get; set; }
        public ICollection<InsumoDetalleOrden> InsumoOrdenes { get; set; }
    }
}