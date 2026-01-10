namespace DTO.ObjetosDTO
{
    public class EstanteDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime? FechaBaja { get; set; } // 👈 opcional pero MUY recomendable
        public decimal CantidadInsumo { get; set; }
        public string NombreInsumo { get; set; }
        public int? InsumoId { get; set; }

    }
}
