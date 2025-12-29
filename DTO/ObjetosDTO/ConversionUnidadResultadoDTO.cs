namespace DTO.ObjetosDTO
{
    public class ConversionUnidadResultadoDTO
    {
        public int UnidadOrigenId { get; set; }
        public string UnidadOrigenNombre { get; set; }
        public string UnidadOrigenAbreviatura { get; set; }
        public int UnidadDestinoId { get; set; }
        public string UnidadDestinoNombre { get; set; }
        public string UnidadDestinoAbreviatura { get; set; }
        public decimal CantidadOrigen { get; set; }
        public decimal CantidadConvertida { get; set; }
        public string UnidadConvertidaTexto { get; set; }
    }
}
