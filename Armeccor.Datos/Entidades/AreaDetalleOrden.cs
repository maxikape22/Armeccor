namespace Armeccor.Datos.Entidades
{
    public class AreaDetalleOrden
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Area Area { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; }
        public int Tiempo { get; set; }
        public int Prioridad { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }

    }
}
