using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class ConvertirUnidadDTO
    {
        public decimal Cantidad { get; set; }

        public int UnidadOrigenId { get; set; }
        public int UnidadDestinoId { get; set; }
    }

}
