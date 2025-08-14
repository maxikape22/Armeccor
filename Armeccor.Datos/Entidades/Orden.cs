using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    //NroOT y CodigoOrden son claves unicas
    [Index(nameof(CodigoOrden), Name = "CodigoOrden_UQ", IsUnique = true)]
    [Index(nameof(NroOT), Name = "NroOT_UQ", IsUnique = true)]

    public class Orden
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CodigoOrden { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prioridad { get; set; } // Alta, Media, Baja

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; } // Abierto, Cerrado, Cancelado

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required]
        public int NroOT { get; set; }
        public int IdCliente { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        public HashSet<AreaOrden> AreaOrdenes { get; set; }
        public HashSet<Pieza> Piezas { get; set; }
        public HashSet<Entrega> Entregas { get; set; }
        public HashSet<InsumoOrden> InsumosOrden { get; set; }
    }
}
