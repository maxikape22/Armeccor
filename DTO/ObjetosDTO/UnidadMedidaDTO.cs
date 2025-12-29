using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class UnidadMedidaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public string Tipo { get; set; } // Ej: "Volumen", "Masa", "Longitud", "Unidad"
    }

}
