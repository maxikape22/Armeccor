namespace DTO.ObjetosDTO
{
    public class AreaDetalleOrdenDTO
    {
        public Nullable<int> OrdenId { get; set; }
        public int? NroOT { get; set; } // 👈 nueva propiedad
        public Nullable<int> AreaId { get; set; }
        public string NombreArea { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; }
        public int Tiempo { get; set; }
    }
}
