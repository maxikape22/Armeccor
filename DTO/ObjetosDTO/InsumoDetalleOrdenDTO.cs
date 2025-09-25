namespace DTO.ObjetosDTO
{
    public class InsumoDetalleOrdenDTO
    {
        public int Id { get; set; }

        public int InsumoId { get; set; }
        public int OrdenId { get; set; }
        public int Cantidad { get; set; }
    }
}