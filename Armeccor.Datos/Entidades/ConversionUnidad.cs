using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    public class UnidadConversion
    {
        public int Id { get; set; }

        public int UnidadOrigenId { get; set; }
        public UnidadMedida UnidadOrigen { get; set; }

        public int UnidadDestinoId { get; set; }
        public UnidadMedida UnidadDestino { get; set; }
        public decimal FactorConversion { get; set; }
        // Ej: 1 caja = 100 unidades → Factor = 100
    }
}
