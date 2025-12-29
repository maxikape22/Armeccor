using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class InsumoConConversionesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        public decimal CantDisponible { get; set; }
        public int UnidadMedidaId { get; set; }
        public string UnidadMedidaNombre { get; set; }
        public string UnidadMedidaAbreviatura { get; set; }
        public List<ConversionUnidadResultadoDTO> Conversiones { get; set; }
        public decimal CantidadConvertida { get; set; }
        public string Item { get; set; }
        public string UnidadConvertidaTexto { get; set; }
        public string Tipo { get; set; }

        //

        private string _tipoDestino = "";
        public string TipoDestino
        {
            get => _tipoDestino;
            set
            {
                _tipoDestino = value;
                // cada vez que cambia, recalculamos unidades destino
            }
        }

        private string _tipoOrigen = "";
        public string TipoOrigen
        {
            get => _tipoOrigen;
            set
            {
                _tipoOrigen = value;
                // cada vez que cambia, recalculamos unidades destino
            }
        }
        public int UnidadDestinoId { get; set; }
        public int UnidadOrigenId { get; set; }
        public List<UnidadDestinoItem> UnidadesOrigen { get; set; } = new();
        public List<UnidadDestinoItem> UnidadesDestino { get; set; } = new();

    }

    public class UnidadDestinoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Abreviatura { get; set; } = "";
    }

}
