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
    }
}