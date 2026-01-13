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
        public TimeSpan Tiempo { get; set; }
        public string TiempoIngresado { get; set; } = "00:00:00";
        public int CantidadADescontar { get; set; } = 1;
        public int Prioridad { get; set; }
        public string PrioridadTexto { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }
        public bool EstaCorriendo { get; set; }
        public string EstadoOrden { get; set; }
    }
}
