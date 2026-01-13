using System.ComponentModel.DataAnnotations.Schema;

namespace DTO.ObjetosDTO
{
    public class AreaDetalleOrdenDTO
    {
        public Nullable<int> OrdenId { get; set; }
        public int? NroOT { get; set; } // 👈 nueva propiedad
        public string NombreOrden { get; set; }
        public string NombreCliente { get; set; }
        public Nullable<int> AreaId { get; set; }
        public string NombreArea { get; set; }
        public string? Descripcion { get; set; }
        public string Estado { get; set; }
        public TimeSpan Tiempo { get; set; }
        public string Comentario { get; set; }
    }
}
