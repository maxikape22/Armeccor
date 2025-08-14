using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    public class AreaOrden
    {
        public int Id { get; set; }

        [ForeignKey("Area")]
        public int IdArea { get; set; }
        public Area Area { get; set; }

        [ForeignKey("Orden")]
        public int IdOrden { get; set; }
        public Orden Orden { get; set; }
    }
}
