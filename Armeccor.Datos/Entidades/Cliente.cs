using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression(@"^\d{7,8}$",
        ErrorMessage = "El DNI debe contener solo números y tener entre 7 y 8 dígitos")]
        public int DNI { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        [RegularExpression(@"^(?:\+54|54)?(?:9)?(?:0)?(?:15)?\d{6,10}$", 
        ErrorMessage 
        = "Formato de teléfono inválido para Argentina. Ingrese todo junto, sin espacios ni guiones.")]
        public string Telefono { get; set; }
        public ICollection<Orden> Ordenes { get; set; }
    }
}