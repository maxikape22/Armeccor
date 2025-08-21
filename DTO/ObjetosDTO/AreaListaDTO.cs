using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ObjetosDTO
{
    public class AreaListaDTO
    {
        public int Id { get; set; }
        public string NombreArea { get; set; }
        public int Tiempo { get; set; }
        public string Estado { get; set; }
    }
}


  