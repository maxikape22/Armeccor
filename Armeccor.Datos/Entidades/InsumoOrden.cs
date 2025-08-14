using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    public class InsumoOrden
    {
        public int Id { get; set; }
        [ForeignKey("Insumo")]
        public int IdInsumo { get; set; }
        public Insumo Insumo { get; set; }
        [ForeignKey("Orden")]
        public int IdOrden { get; set; }
        public Orden Orden { get; set; }
        public int CantidadUsada { get; set; }
    }
}
