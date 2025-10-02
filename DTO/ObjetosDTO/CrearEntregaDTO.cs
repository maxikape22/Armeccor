namespace DTO.ObjetosDTO
{
    public class CrearEntregaDTO
    {
        public int MedioDePagoId { get; set; }   // clave foránea a MedioDePago
        public bool Entregado { get; set; }
        public int IdOrden { get; set; }
        public string? NuevoMedio { get; set; }   // opcional, solo si el usuario elige "Otro medio"
        public string MedioDePago { get; set; }
    }
}
