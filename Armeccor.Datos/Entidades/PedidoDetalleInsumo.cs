namespace Armeccor.Datos.Entidades
{
    public class PedidoDetalleInsumo
    {
        public int Id { get; set; }
        public int? IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public int? IdInsumo { get; set; }
        public Insumo Insumo { get; set; }
        public string Item { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaUso { get; set; }
        public bool EsSolicitado { get; set; }
        public string Estado { get; set; } //--> PARA RECIBIR EL ESTADO; EJ:° --> "RECIBIDO"
        public bool EntregaParcial { get; set; }
        public bool EntregaTotal { get; set; }
    }
}
