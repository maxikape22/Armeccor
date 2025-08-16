using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Area
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string NombreArea { get; set; }
        [Required]
        public int Tiempo { get; set; }
        [Required]
        public string Estado { get; set; }
        public ICollection<Orden> Ordenes { get; set; }
    }
}
