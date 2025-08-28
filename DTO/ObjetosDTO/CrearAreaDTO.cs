using System.ComponentModel.DataAnnotations;

namespace DTO.ObjetosDTO
{
    public class CrearAreaDTO
    {
        [MaxLength(100)]
        public string NombreArea { get; set; }
    }
}