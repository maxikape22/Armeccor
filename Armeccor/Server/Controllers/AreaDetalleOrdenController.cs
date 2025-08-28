using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Area_Detalle_Orden")]
    public class AreaDetalleOrdenController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        public AreaDetalleOrdenController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> GetListaAreaDetalleOrdenDTO()
        {
            var AreaDetalleOrdenDTO = await context.AreaDetalleOrdenes
            .Select(x => new AreaDetalleOrdenListaDTO       
            { 
                Id = x.Id,
                OrdenId = x.OrdenId,
                AreaId = x.AreaId, 
                Descripcion = x.Descripcion ,
                Estado = x.Estado, 
                Tiempo = x.Tiempo
                
            }).ToListAsync();
            await context.AreaDetalleOrdenes.ToListAsync();
            return Ok(AreaDetalleOrdenDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AreaDetalleOrden>> PostAreaDetalleOrdenDTO(AreaDetalleOrdenDTO AreaDetalleOrdenDTO)
        {
            var AreaDetalleOrden = _mapper.Map<AreaDetalleOrden>(AreaDetalleOrdenDTO);
            context.AreaDetalleOrdenes.Add(AreaDetalleOrden);
            await context.SaveChangesAsync();
            var areaDetalleOrdenDTO = _mapper.Map<AreaDetalleOrden>(AreaDetalleOrden);
            return Ok(areaDetalleOrdenDTO);
        }

        [HttpPost("VariasAreasTablaIntermediaEnOrden")]
        public async Task<ActionResult> Post(AreaDetalleOrdenDTO[] areas)
        {
            context.AddRange(areas);
            await context.SaveChangesAsync();
            return Ok();
        }

        //[HttpGet("AreaDetalleOrdenConNombreArea")]
        //public async Task<ActionResult<List<AreaDetalleOrdenDTO>>> GetAreasDetalleConNombre()
        //{
        //    var areasConNombre = await context.AreaDetalleOrdenes
        //        .Join(context.Areas, // Tabla de Áreas
        //              detalle => detalle.AreaId, // Clave foránea en AreaDetalleOrden
        //              area => area.Id, // Clave primaria en Areas
        //              (detalle, area) => new AreaDetalleOrdenDTO
        //              {
        //                  AreaId = detalle.AreaId,
        //                  OrdenId = detalle.OrdenId,
        //                  Descripcion = detalle.Descripcion,
        //                  Estado = detalle.Estado,
        //                  Tiempo = detalle.Tiempo,
        //                  //Area = new CrearAreaDTO()
        //                  //{
        //                  //      NombreArea = area.NombreArea // Asignar el nombre del área
        //                  //}
        //                  NombreArea = area.NombreArea // Asignar el nombre del área
        //              })
        //        .ToListAsync();

        //    return Ok(areasConNombre);
        //}


    }
}
