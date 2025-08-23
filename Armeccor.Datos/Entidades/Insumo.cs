using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Insumo
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public string UnidadMedida { get; set; }
        public int CantDisponible { get; set; }
        public string Tipo { get; set; }
        public ICollection<InsumoDetalleOrden> InsumoOrdenes{ get; set; }
    }
}