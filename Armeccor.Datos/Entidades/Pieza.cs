using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    //Codigo es clave unica
    [Index(nameof(Codigo), Name = "Codigo_UQ", IsUnique = true)]

    public class Pieza
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Codigo { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        [Required]

        [MaxLength(250)]
        public string Descripcion { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Precio { get; set; }

        [ForeignKey("Orden")]
        public int IdOrden { get; set; }
        public Orden Orden { get; set; }
    }
}
