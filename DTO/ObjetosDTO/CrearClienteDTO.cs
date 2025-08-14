using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DTO.ObjetosDTO
{
    public class CrearClienteDTO
    {
        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El documento del cliente es obligatorio.")]
        public int DNI { get; set; }
        [Required(ErrorMessage = "La dirección del cliente es obligatoria.")]
        public string Dirección { get; set; }
        [Required(ErrorMessage = "El telefono del cliente es obligatorio.")]
        public int Telefono { get; set; }
    }
}
