using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armeccor.Datos.Entidades
{
    public class InsumoOrden
    {
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public int InsumoId { get; set; }
        public Insumo Insumo { get; set; }
        public int CantidadUsada { get; set; }
    }
}
