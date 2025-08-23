using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class AreaDetalleOrdenListaDTO
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int AreaId { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int Tiempo { get; set; }
    }
}
