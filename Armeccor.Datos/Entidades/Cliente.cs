using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public int DNI { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Telefono { get; set; }
        public ICollection<Orden> Ordenes { get; set; }
    }
}