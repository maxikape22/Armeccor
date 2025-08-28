using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DTO.ObjetosDTO
{
    public class CrearOrdenDTO
    {
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
        public Nullable<int> PlanoId { get; set; } = null; // puede ser null si no se cargó aún                                    
    }
}
