namespace DTO.ObjetosDTO
{
    public class PedidoDetalleInsumoDTO
    {
        public int Id { get; set; }
        public int? IdPedido { get; set; }
        public int? IdInsumo { get; set; }
        public string Item { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaUso { get; set; }
        public int NroOT { get; set; } // --> Viene mapeado de CrearInsumoDTO o algún DTO de Orden que rellene este campo
        public bool EsSolicitado { get; set; }
        public string Estado { get; set; } //--> PARA RECIBIR EL ESTADO; EJ:° --> "RECIBIDO"
        public string NombreInsumoInsuficiente { get; set; } // <-- NUEVO
        public int NroPedido { get; set; }
        public bool EntregaParcial { get; set; }
        public bool EntregaTotal { get; set; }
        public string NuevoEstado { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }
    }
}
