namespace Armeccor.Datos.Entidades
{
    public class InsumoDetalleOrden
    {
        public int Id { get; set; }
        public int InsumoId { get; set; }
        public Insumo Insumo { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public int Cantidad { get; set; }
    }
}
