using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Insumo
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nombre { get; set; }
        public int CantidadDisponible { get; set; }
        public HashSet<InsumoOrden> InsumosOrden { get; set; }
    }
}
