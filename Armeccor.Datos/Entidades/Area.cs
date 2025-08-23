using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Area
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string NombreArea { get; set; }
        public ICollection<AreaDetalleOrden> AreaOrdenes { get; set; }
    }
}
