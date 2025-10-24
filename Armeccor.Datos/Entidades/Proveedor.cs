namespace Armeccor.Datos.Entidades
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
