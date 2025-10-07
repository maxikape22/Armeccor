namespace DTO.ObjetosDTO
{
    public class AreaDetalleOrdenListaDTO
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public Nullable<int> AreaId { get; set; }
        public string NombreArea { get; set; }
        public string NombreOrden { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Tiempo { get; set; }
        public string Comentario { get; set; }
    }
}
