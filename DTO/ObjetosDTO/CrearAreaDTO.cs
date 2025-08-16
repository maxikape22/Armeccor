using System.ComponentModel.DataAnnotations;

namespace DTO.ObjetosDTO
{
    public class CrearAreaDTO
    {
        [Required]
        [MaxLength(100)]
        public string NombreArea { get; set; }
        [Required]
        public int Tiempo { get; set; }
        [Required]
        public string Estado { get; set; }
    }
}