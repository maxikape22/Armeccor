namespace DTO.ObjetosDTO
{
    public class InsumoDetalleOrdenDTO
    {
        public int Id { get; set; }
        public int NroOT { get; set; } // 👈 nueva propiedad
        public int InsumoId { get; set; }
        public int OrdenId { get; set; }
        public int Cantidad { get; set; }
        public string Nombre { get; set; } // borrar?

        // ✅ Propiedad temporal para el textbox de liberar
        public int? CantidadALiberar { get; set; }
        public int Saldo { get; set; }
        public int CantidadDescontada { get; set; }
        public int CantidadPendiente { get; set; }
        public bool Insuficiente { get; set; }

    }
}