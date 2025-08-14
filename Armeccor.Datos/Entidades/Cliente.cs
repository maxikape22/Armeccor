using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public  int DNI { get; set; }
        [Required]
        public string Dirección { get; set; }
        [Required]
        public int Telefono { get; set; }
        public HashSet<Orden> Ordenes { get; set; }
    }
}