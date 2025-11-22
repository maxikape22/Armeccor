namespace Armeccor.Datos.Entidades
{ 
    public class Notificacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; } // 'Insumo', 'Orden', 'Area'
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool EsLeida { get; set; }
    }
}