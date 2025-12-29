using Microsoft.EntityFrameworkCore;

namespace Armeccor.Datos.Entidades
{
    [Index(nameof(NroPedido), Name = "NroPedido_UQ", IsUnique = true)]
    public class Pedido
    {
        public int Id { get; set; }
        public int NroPedido { get; set; } //--> se debe generar aleatoriamente y no se repita
        public string Estado { get; set; } //--> PARA ENVIAR EL ESTADO; EJ:° --> "ENVIADO A PROVEEDOR"
        public int? IdProveedor { get; set; }
        public Proveedor Proveedor { get; set; }
        public ICollection<PedidoDetalleInsumo> DetallePedidos { get; set; }
    }
}