using System.ComponentModel.DataAnnotations;

namespace Armeccor.Datos.Entidades
{
    public class UnidadMedida
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }       // "Kilogramo", "Metro", "Unidad"
        public string Abreviatura { get; set; }  // kg, m, un
        public bool EsBase { get; set; }          // kg, m, l, un
        public string Tipo { get; set; } // Ej: "Volumen", "Masa", "Longitud", "Unidad"
        public int? UnidadesPorCaja { get; set; } // Ej: 100
        public int? UnidadesPorAgrupacion { get; set; } // para caja, paquete, bolsa
        public decimal? LongitudPorUnidad { get; set; } // para barra, perfil, planchuela

    }
}
