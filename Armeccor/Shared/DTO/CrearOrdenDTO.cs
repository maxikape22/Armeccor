using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armeccor.Shared.DTO
{
    public class CrearOrdenDTO
    {
        [Required(ErrorMessage ="El código de la orden es obligatorio")]
        [MaxLength(50)]
        public string CodigoOrden { get; set; }

        [Required(ErrorMessage ="La prioridad es de ingreso obligatorio")]
        [MaxLength(50)]
        public string Prioridad { get; set; } 

        [Required(ErrorMessage ="El ingreso del estado es de caracter obligatorio")]
        [MaxLength(50)]
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required(ErrorMessage ="El número de la orden es obligatorio")]
        public int NroOT { get; set; }
    }
}
