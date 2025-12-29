namespace DTO.ObjetosDTO
{
    public class CrearInsumoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public decimal CantDisponible { get; set; }
        public int NroOT { get; set; }

        //prueba para el textbox de agregarInsumoOrden
        public int? CantidadARetirar { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public int UnidadMedidaId { get; set; }
        public string NombreCombinadoUnidadMedida { get; set; } // --> COMO LO DICE SU NOMBRE ESTO ES UN CAMPO QUE NO EXISTE EN LA BASE DE DATOS PERO LO VOY A USAR PARA COMBINAR EL NOMBRE DEL INSUMO CON EL DETALLE QUE PODRIA SER COMO UN ITEM , POR EJ°: Tornillos 1/4 x 6 --> solo se va a listar producto del mapeo en automapper cuando se haga el post del nuevo insumo
        public string UnidadMedidaConvertida { get; set; }
    }
}