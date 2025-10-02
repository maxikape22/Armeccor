using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class NotificacionDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool EsLeida { get; set; }
        public int? InsumoId { get; set; }
        public int? OrdenId { get; set; }
        public int? AreaDetalleId { get; set; }
    }
}
