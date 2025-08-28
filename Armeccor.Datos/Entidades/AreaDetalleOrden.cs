using Validaciones.CamposAreaDetalleOrden;

namespace Armeccor.Datos.Entidades
{
    public class AreaDetalleOrden
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; }
        [Tiempo]
        public int Tiempo { get; set; }
    }
}
