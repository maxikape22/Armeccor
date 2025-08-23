using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Validaciones.CamposAreaDetalleOrden
{
    public class TiempoAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult("El campo Tiempo es obligatorio.");

            string input = value.ToString().Trim();

            // Expresiones regulares: "número + h" o "número + m"
            var regexHoras = new Regex(@"^(\d+)h$", RegexOptions.IgnoreCase);
            var regexMinutos = new Regex(@"^(\d+)m$", RegexOptions.IgnoreCase);

            if (regexHoras.IsMatch(input))
            {
                int horas = int.Parse(regexHoras.Match(input).Groups[1].Value);

                // Validación: 1h hasta 24h aceptadas directamente
                if (horas >= 1 && horas <= 24)
                    return ValidationResult.Success;

                // Si es mayor a 24h, lo aceptamos como múltiplos de 60 minutos
                if (horas > 24)
                    return ValidationResult.Success;

                return new ValidationResult("Las horas deben ser entre 1 y 24.");
            }
            else if (regexMinutos.IsMatch(input))
            {
                int minutos = int.Parse(regexMinutos.Match(input).Groups[1].Value);

                if (minutos >= 0 && minutos < 60)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Los minutos deben estar entre 0 y 59.");
            }

            return new ValidationResult("Formato inválido. Use '12h' para horas o '30m' para minutos.");
        }
    }
}
