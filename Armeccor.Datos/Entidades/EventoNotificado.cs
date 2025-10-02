namespace Armeccor.Datos.Entidades
{
    public class EventoNotificado
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Insumo", "Orden", "Área"
        public int ReferenciaId { get; set; } // InsumoId, OrdenId, AreaDetalleId
        public DateTime Fecha { get; set; }
    }
}