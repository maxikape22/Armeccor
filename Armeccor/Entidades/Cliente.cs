using System.ComponentModel.DataAnnotations;

namespace Armeccor.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }

    }
}
