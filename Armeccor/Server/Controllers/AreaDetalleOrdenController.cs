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
    public class AreaDetalleOrdenController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public AreaDetalleOrdenController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        // En tu controlador API del servidor
        [HttpGet("{nroOT}")]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetAreasByNroOT(int nroOT)
        {
            var orden = await context.Ordenes
                .Where(o => o.NroOT == nroOT)
                .FirstOrDefaultAsync();

            if (orden == null)
            {
                return NotFound($"No se encontró la orden con el número de OT: {nroOT}.");
            }

            var areasDetalle = await context.AreaDetalleOrdenes
                .Where(ado => ado.OrdenId == orden.Id)
                .Include(ado => ado.Area)
                .Select(ado => new AreaDetalleOrdenListaDTO
                {
                    Id = ado.Id,
                    NombreArea = ado.Area.NombreArea,
                    Descripcion = ado.Descripcion,
                    Estado = ado.Estado,
                    Tiempo = ado.Tiempo
                })
                .ToListAsync();

            return Ok(areasDetalle);
        }

        [HttpGet]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetLista()
        {
            var areaDetalleOrdenes = await context.AreaDetalleOrdenes.Include(a => a.Area).ToListAsync();
            var areaDetalleOrdenesDTO = _mapper.Map<List<AreaDetalleOrdenListaDTO>>(areaDetalleOrdenes);
            return Ok(areaDetalleOrdenesDTO);
        }

        [HttpGet("Original")]
        public async Task<ActionResult<List<AreaDetalleOrden>>> GetAreaDetalleOriginal()
        {
            var areaDetalleOrdenes = await context.AreaDetalleOrdenes.ToListAsync();
            return Ok(areaDetalleOrdenes);
        }

        [HttpGet("Orden")]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetAreaDetallada()
        {
            var area = new AreaDTO();
            var lista = await context.AreaDetalleOrdenes
                .Where(ado => ado.OrdenId == area.Id)
                .Include(ado => ado.Area)
                .Select(ado => new AreaDetalleOrdenListaDTO
                {
                    Id = ado.Id,
                    //OrdenId = ado.OrdenId,
                    //AreaId = ado.AreaId,
                    Descripcion = ado.Descripcion,
                    Estado = ado.Estado,
                    Tiempo = ado.Tiempo
                    //NombreArea = ado.Area.NombreArea // 👈 acá se resuelve
                })
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<AreaDetalleOrden>> AreaDetalleOrdenDTO(AreaDetalleOrdenDTO dto)
        {
            // Validación solo si vienen valores
            if (dto.AreaId.HasValue)
            {
                var areaExiste = await context.Areas.AnyAsync(a => a.Id == dto.AreaId.Value);
                if (!areaExiste)
                    return BadRequest($"El Área con Id {dto.AreaId} no existe.");
            }

            if (dto.OrdenId.HasValue)
            {
                var ordenExiste = await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId.Value);
                if (!ordenExiste)
                    return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");
            }
            var entity = _mapper.Map<AreaDetalleOrden>(dto);

            context.AreaDetalleOrdenes.Add(entity);
            await context.SaveChangesAsync();

            var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(entity);
            return Ok(resultDto);
        }

        [HttpPost("AreaDetallaEnOrden")]
        public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        {
            // Validar que existan las relaciones
            if (!await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId))
                return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");

            if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
                return BadRequest($"El Área con Id {dto.AreaId} no existe.");

            var entity = _mapper.Map<AreaDetalleOrden>(dto);
            context.AreaDetalleOrdenes.Add(entity);

            await context.SaveChangesAsync();

            // Incluir el Nombre del área para devolverlo al front
            var result = await context.AreaDetalleOrdenes
                .Include(x => x.Area)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
            resultDto.NombreArea = result.Area?.NombreArea;

            return Ok(resultDto);
        }


        //[HttpPut("{id}/Estado")]
        //public async Task<ActionResult> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        //{
        //    var areaDetalle = await context.AreaDetalleOrdenes.FindAsync(id);
        //    if (areaDetalle == null) return NotFound();

        //    if (!string.IsNullOrEmpty(dto.Estado))
        //        areaDetalle.Estado = dto.Estado;

        //    await context.SaveChangesAsync();
        //    return Ok(areaDetalle);
        //}

        [HttpPut("{id}/Estado")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        {
            var areaDetalle = await context.AreaDetalleOrdenes.Include(a => a.Area).FirstOrDefaultAsync(x => x.Id == id);
            if (areaDetalle == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.Estado))
                areaDetalle.Estado = dto.Estado;

            await context.SaveChangesAsync();

            var result = new AreaDetalleOrdenListaDTO
            {
                Id = areaDetalle.Id,
                OrdenId = areaDetalle.OrdenId,
                AreaId = areaDetalle.AreaId,
                Descripcion = areaDetalle.Descripcion,
                Estado = areaDetalle.Estado,
                Tiempo = areaDetalle.Tiempo,
                NombreArea = areaDetalle.Area?.NombreArea
            };

            return Ok(result);
        }

        [HttpGet("AreaActual/{nroOT}")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> GetAreaActual(int nroOT)
        {
            var orden = await context.Ordenes
                .FirstOrDefaultAsync(o => o.NroOT == nroOT);

            if (orden == null)
                return NotFound($"No existe la orden con NroOT {nroOT}");

            var areaDetalle = await context.AreaDetalleOrdenes
                .Include(a => a.Area)
                .Where(z => z.OrdenId == orden.Id && z.Estado == "Iniciado")
                .OrderBy(z => z.Id)
                .FirstOrDefaultAsync();

            if (areaDetalle == null)
                return NotFound("No hay áreas en estado 'Iniciado' para esta orden.");

            return Ok(new AreaDetalleOrdenListaDTO
            {
                Id = areaDetalle.Id,
                OrdenId = areaDetalle.OrdenId,
                AreaId = areaDetalle.AreaId,
                Descripcion = areaDetalle.Descripcion,
                Estado = areaDetalle.Estado,
                Tiempo = areaDetalle.Tiempo,
                NombreArea = areaDetalle.Area?.NombreArea
            });
        }




        // DELETE: api/Area_Detalle_Orden/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarArea(int id)
        {
            var areaDetalle = await context.AreaDetalleOrdenes.FindAsync(id);
            if (areaDetalle == null)
                return NotFound("Área detallada no encontrada");

            // Set null en las relaciones si corresponde
            areaDetalle.OrdenId = 0;
            areaDetalle.AreaId = null;

            context.AreaDetalleOrdenes.Remove(areaDetalle);
            await context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("EstadoDeUnArea")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> GetEstadoDeUnArea()
        {
            var areaDetalle = await context.AreaDetalleOrdenes.Where(z=>z.Estado == "Iniciado").ToListAsync();
                //.FirstOrDefaultAsync(ado => ado.Estado == "Pendiente");
            return Ok(areaDetalle);

        }

        [HttpGet("EstadoDeUnArea/{ordenId}")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> GetEstadoDeUnArea(int ordenId)
        {
            var areaDetalle = await context.AreaDetalleOrdenes
                .Include(a => a.Area)
                .Where(z => z.OrdenId == ordenId && z.Estado == "Iniciado")
                .OrderBy(z => z.Id) // opcional: la primera en iniciarse
                .FirstOrDefaultAsync();

            if (areaDetalle == null)
                return NotFound("No hay áreas en estado 'Iniciado' para esta orden.");

            return Ok(new AreaDetalleOrdenListaDTO
            {
                Id = areaDetalle.Id,
                OrdenId = areaDetalle.OrdenId,
                AreaId = areaDetalle.AreaId,
                Descripcion = areaDetalle.Descripcion,
                Estado = areaDetalle.Estado,
                Tiempo = areaDetalle.Tiempo,
                NombreArea = areaDetalle.Area?.NombreArea
            });
        }

    }
}
