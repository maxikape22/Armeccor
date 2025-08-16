using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Insumo
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        public int CantDisponible { get; set; }
        public ICollection<InsumoOrden> InsumosOrden { get; set; }
    }
}