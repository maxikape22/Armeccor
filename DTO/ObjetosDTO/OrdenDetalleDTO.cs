using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class OrdenDetalleDTO
    {
        public int Id { get; set; }
        public int NroOT { get; set; }
        [Required]
        public string NombreOrden { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required, MaxLength(50)]
        public string Estado { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaPactada { get; set; } = DateTime.Now;
        public Nullable<DateTime> FechaEntrega { get; set; }
        public int AreaId { get; set; }
        public int ClienteId { get; set; }
        public int? PlanoId { get; set; } // puede ser null si no se cargó aún
        public string NombreCliente { get; set; } // Aplanado del Cliente
        public string NombreArea { get; set; } //Aplanado de Area
        public List<AreaOrdenDetalleDTO> AreasDeLaOrden { get; set; }
        public List<CrearEntregaDTO> EntregasDeLaOrden { get; set; }
    }

}
