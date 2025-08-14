using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    public class Area
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string NombreArea { get; set; }

        [Required]
        public int Tiempo { get; set; } 

        // FK Orden
        public int IdOrden { get; set; }
        [ForeignKey("IdOrden")]
        public Orden Orden { get; set; }
        public HashSet<AreaOrden> AreasOrden { get; set; }

    }
}
