using System.ComponentModel.DataAnnotations;

namespace DTO.ObjetosDTO
{
    public class CrearOrdenDTO
    {
        [Required(ErrorMessage = "El código de la orden es obligatorio")]
        [MaxLength(50)]
        public string CodigoOrden { get; set; }

        [Required(ErrorMessage = "La prioridad es de ingreso obligatorio")]
        [MaxLength(50)]
        public string Prioridad { get; set; }

        [Required(ErrorMessage = "El ingreso del estado es de caracter obligatorio")]
        [MaxLength(50)]
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El número de la orden es obligatorio")]
        public int NroOT { get; set; }
        public int IdCliente { get; set; }
    }
}
