namespace DTO.ObjetosDTO
{
    public class OrdenDTO
    {
        public int Id { get; set; }
        public int NroOT { get; set; }
        public string NombreOrden { get; set; }
        public List<AreaOrdenDetalleDTO> AreaDetalleOrdenes { get; set; }
    }
   
}
