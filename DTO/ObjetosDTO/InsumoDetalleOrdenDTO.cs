namespace DTO.ObjetosDTO
{
    public class InsumoDetalleOrdenDTO
    {
        public int Id { get; set; }
        public int? NroOT { get; set; } // 👈 nueva propiedad
        public int InsumoId { get; set; }
        public int OrdenId { get; set; }
        public int Cantidad { get; set; }

        public string Nombre { get; set; } // borrar?


    }
}