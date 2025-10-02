using Microsoft.EntityFrameworkCore;

namespace Armeccor.Datos.Entidades
{
    [Index(nameof(Nombre_Medio), Name = "Nombre_Medio_UQ", IsUnique = true)]
    public class MedioDePago
    {
        public int Id { get; set; }
        public string Nombre_Medio { get; set; }
        public ICollection<Entrega> Entregas { get; set; }
    }
}
