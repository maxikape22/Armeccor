using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class RespuestaPaginada
    {
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalPaginas { get; set; }
        public List<OrdenDetalleDTO> Datos { get; set; } = new();
    }

}
