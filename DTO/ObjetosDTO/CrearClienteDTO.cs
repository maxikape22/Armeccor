using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO.ObjetosDTO
{
    public class CrearClienteDTO
    {
        public string Nombre { get; set; }
        public int DNI { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool EstaActivo { get; set; }
    }
}
