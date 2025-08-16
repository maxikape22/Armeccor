using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armeccor.Datos.Entidades
{
    public class Entrega
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string MedioDePago { get; set; }
        public bool Entregado { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
    }

}