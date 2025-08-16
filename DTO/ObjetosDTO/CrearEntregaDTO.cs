namespace DTO.ObjetosDTO
{
    public class CrearEntregaDTO 
    {
        public string MedioDePago { get; set; } = string.Empty;
        public bool Entregado  { get; set; }
        public int IdOrden { get; set; }
    }

}
