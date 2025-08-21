using System.ComponentModel.DataAnnotations;

namespace DTO.ObjetosDTO
{
    public class InsumoOrdenDTO
    {
        [Required]
        public int OrdenId { get; set; }
        [Required]
        public int InsumoId { get; set; }
    }
}