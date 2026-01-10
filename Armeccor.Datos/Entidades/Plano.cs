using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Plano
    {
        public int Id { get; set; }
        [Required]
        public string RutaSVG { get; set; }
        public string RutaOriginal { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        // 🔑 Clave foránea hacia Orden
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }
    }
}