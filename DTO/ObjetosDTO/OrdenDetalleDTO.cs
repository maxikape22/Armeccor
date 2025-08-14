using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    // DTO.ObjetosDTO/OrdenDetalleDTO.cs (y sus DTOs anidados)
    public class OrdenDetalleDTO
    {
        public int Id { get; set; }
        public string CodigoOrden { get; set; }
        public string Prioridad { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int NroOT { get; set; }

        public string NombreCliente { get; set; } // Aplanado del Cliente
        public int IdCliente { get; set; }
        public List<AreaOrdenDetalleDTO> AreasDeLaOrden { get; set; }
        public List<PiezaDetalleDTO> PiezasDeLaOrden { get; set; }
        public List<EntregaDetalleDTO> EntregasDeLaOrden { get; set; }
        public List<InsumoOrdenDetalleDTO> InsumosDeLaOrden { get; set; }
    }

}
