namespace DTO.ObjetosDTO
{
    public class ConversionUnidadRequestDTO
    {
        public decimal Cantidad { get; set; }
        // Desde qué unidad estoy partiendo (lo que el usuario eligió)
        public int UnidadOrigenId { get; set; }
        // Si es null → convertir a TODAS las unidades conectadas
        public int? UnidadDestinoId { get; set; }
    }
}
