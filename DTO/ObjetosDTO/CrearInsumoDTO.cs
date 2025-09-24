namespace DTO.ObjetosDTO
{
    public class CrearInsumoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CantDisponible { get; set; }

        //prueba para el textbox de agregarInsumoOrden
        public int? CantidadARetirar { get; set; }

    }
}