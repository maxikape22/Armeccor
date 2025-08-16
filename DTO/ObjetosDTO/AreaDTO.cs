using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class AreaDTO // DTO para listar áreas (solo Id y Nombre)
    {
        public int Id { get; set; }
        public string? NombreArea { get; set; } // Asegúrate de que este nombre coincida con tu entidad Area
    }
}
