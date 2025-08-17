using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class EntregaDetalleDTO
    {
        public int Id { get; set; }
        public DateTime FechaEntrega { get; set; }  // viene de Orden.FechaEntrega o FechaPactada
        public bool Entregado { get; set; }
        public string MedioDePago { get; set; } = string.Empty;
    }

}
