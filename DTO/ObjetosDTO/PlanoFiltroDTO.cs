namespace DTO.ObjetosDTO
{
    public class PlanoFiltroDTO
    {
        public int Id { get; set; }
        public string RutaSVG { get; set; }
        public string RutaOriginal { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string NombreOrden { get; set; }
        public int OrdenId { get; set; }
        public int NroOT { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }
    }
}
