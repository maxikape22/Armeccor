namespace DTO.ObjetosDTO
{
    // Este DTO debe estar en tu proyecto, en la carpeta de DTOs
    public class InsumoEnOrdenVistaDTO
    {
        // Campos de la Orden
        public int OrdenId { get; set; }
        public int NroOT { get; set; } // Campo de la clase Orden
        public string NombreOrden { get; set; } // Campo de la clase Orden

        // Campos del Insumo
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; } // Se mapeará desde Insumo.Nombre

        // Campo adicional de la tabla intermedia
        public int CantidadUsada { get; set; } // Campo de la tabla InsumoOrden
    }
}
