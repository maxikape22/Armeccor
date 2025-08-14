using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    //CodigoEntrega es clave unica
    [Index(nameof(CodigoEntrega), Name = "CodigoEntrega_UQ", IsUnique = true)]
    public class Entrega
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string CodigoEntrega { get; set; }

        [Required]
        public DateTime FechaEntrega { get; set; }

        [MaxLength(20)]
        public string HorarioEntrega { get; set; }

        [MaxLength(20)]
        public string EstadoEntrega { get; set; }

        [MaxLength(30)]
        public string MedioDePago { get; set; }

        [MaxLength(20)]
        public bool Entregado { get; set; } 

        [ForeignKey("Orden")]
        public int IdOrden { get; set; }
        public Orden Orden { get; set; }
    }
}