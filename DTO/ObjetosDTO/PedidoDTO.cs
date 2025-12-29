namespace DTO.ObjetosDTO
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int NroPedido { get; set; }

        public string Estado { get; set; } //--> PARA ENVIAR EL ESTADO; EJ:° --> "ENVIADO A PROVEEDOR"
        public int? IdProveedor { get; set; }
        public string Nombre { get; set; }
    }
}
