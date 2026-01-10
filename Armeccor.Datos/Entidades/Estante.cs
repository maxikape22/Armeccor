namespace Armeccor.Datos.Entidades
{
    public class Estante
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } //--- 🔒 Borrado lógico
        public DateTime? FechaBaja { get; set; } // 👈 opcional pero MUY recomendable
        public ICollection<Insumo> Insumos { get; set; }
    }
}
