using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class ClienteDTO // DTO para listar clientes (solo Id y Nombre)
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } // Asegúrate de que este nombre coincida con tu entidad Cliente
    }
}
