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
        public int NroOT { get; set; }
        public string NombreOrden { get; set; }  // viene de Orden.FechaEntrega o FechaPactada
        public DateTime? FechaEntrega { get; set; }
        public bool Entregado { get; set; }
        public string MedioDePago { get; set; }
    }

}
