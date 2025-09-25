namespace DTO.ObjetosDTO
{
    public class AgregarInsumoAlaOrden
    {
        public int Id { get; set; }
        public int Insumodd { get; set; }
        public int Ordenid { get; set; }

        //prueba para el textbox de agregarInsumoOrden
        public int? Cantidad { get; set; }

    }
}