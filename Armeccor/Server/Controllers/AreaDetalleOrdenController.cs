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

        [HttpGet("{nroOT}")]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetAreasByNroOT(int nroOT)
        {
            var orden = await context.Ordenes.FirstOrDefaultAsync(o => o.NroOT == nroOT);
            if (orden == null) return NotFound($"No se encontró la orden con el número de OT: {nroOT}.");

            var areasDetalle = await context.AreaDetalleOrdenes
                .Where(a => a.OrdenId == orden.Id)
                .Include(a => a.Area)
                .Include(a => a.Orden.Cliente)
                .Select(a => new AreaDetalleOrdenListaDTO
                {
                    Id = a.Id,
                    OrdenId = a.OrdenId,
                    AreaId = a.AreaId,
                    NombreOrden = orden.NombreOrden,
                    NombreCliente = a.Orden.Cliente.Nombre,
                    NombreArea = a.Area.NombreArea,
                    Descripcion = a.Descripcion,
                    Estado = a.Estado,
                    Tiempo = a.Tiempo,
                    Comentario = a.Comentario,
                    Prioridad = a.Estado == "Finalizado" ? 0 : a.Prioridad
                })
                .ToListAsync();

            return Ok(areasDetalle);
        }

        [HttpGet("OrdenadasPorPrioridad/{nroOT}")]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetAreasOrdenadasPorPrioridad(int nroOT)
        {
            var orden = await context.Ordenes.FirstOrDefaultAsync(o => o.NroOT == nroOT);
            if (orden == null)
                return NotFound($"No se encontró la orden con el número de OT: {nroOT}.");

            var areasDetalle = await context.AreaDetalleOrdenes
                .Where(a => a.OrdenId == orden.Id)
                .Include(a => a.Area)
                .Include(a => a.Orden.Cliente)
                .OrderBy(a => a.Prioridad == 0 ? int.MaxValue : a.Prioridad) // Finalizados al final
                .ThenBy(a => a.Area.NombreArea) // Opcional: orden secundario
                .Select(a => new AreaDetalleOrdenListaDTO
                {
                    Id = a.Id,
                    OrdenId = a.OrdenId,
                    AreaId = a.AreaId,
                    NombreOrden = orden.NombreOrden,
                    NombreCliente = a.Orden.Cliente.Nombre,
                    NombreArea = a.Area.NombreArea,
                    Descripcion = a.Descripcion,
                    Estado = a.Estado,
                    Tiempo = a.Tiempo,
                    Comentario = a.Comentario,
                    Prioridad = a.Estado == "Finalizado" ? 0 : a.Prioridad
                })
                .ToListAsync();

            return Ok(areasDetalle);
        }


        [HttpPut("{id}/Comentario")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] ActualizarComentarioDTO dto)
        {
            var area = await context.AreaDetalleOrdenes.FindAsync(id);
            if (area == null)
                return NotFound("Área no encontrada");

            area.Comentario = dto.Comentario ?? "";
            await context.SaveChangesAsync();

            return Ok("Comentario actualizado correctamente");
        }

        [HttpGet]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetLista()
        {
            var areaDetalleOrdenes = await context.AreaDetalleOrdenes
                .Include(a => a.Area)
                .Include(p=>p.Orden)
                .Include(d=>d.Orden.Cliente)
                .ToListAsync();
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
                    Descripcion = ado.Descripcion,
                    Estado = ado.Estado,
                    Tiempo = ado.Tiempo
                })
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<ActionResult<AreaDetalleOrden>> AreaDetalleOrdenDTO(AreaDetalleOrdenDTO dto)
        {
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
            if (dto.NroOT.HasValue)
            {
                var orden = await context.Ordenes.FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value);
                if (orden == null) return BadRequest($"No existe la orden con NroOT {dto.NroOT}");
                dto.OrdenId = orden.Id;
            }

            if (!await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId))
                return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");

            if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
                return BadRequest($"El Área con Id {dto.AreaId} no existe.");

            // 🚫 Solo bloquear si el nuevo estado es "Iniciado"
            if (dto.Estado == "Iniciado")
            {
                bool yaExisteIniciado = await context.AreaDetalleOrdenes
                    .AnyAsync(a => a.OrdenId == dto.OrdenId && a.AreaId == dto.AreaId && a.Estado == "Iniciado");

                if (yaExisteIniciado)
                    return BadRequest("Ya existe un registro de esta área con estado 'Iniciado' en la misma orden.");
            }

            // ✅ Calcular prioridad como posición en la orden (excluyendo finalizados)
            int prioridad = await context.AreaDetalleOrdenes
                .CountAsync(a => a.OrdenId == dto.OrdenId && a.Estado != "Finalizado") + 1;

            var entity = _mapper.Map<AreaDetalleOrden>(dto);
            entity.Prioridad = dto.Estado == "Finalizado" ? 0 : prioridad;

            context.AreaDetalleOrdenes.Add(entity);
            await context.SaveChangesAsync();

            var result = await context.AreaDetalleOrdenes
                .Include(x => x.Area)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
            resultDto.NombreArea = result.Area?.NombreArea;

            return Ok(resultDto);
        }

        [HttpPut("{id}/Estado")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        {
            var areaDetalle = await context.AreaDetalleOrdenes.Include(a => a.Area).FirstOrDefaultAsync(x => x.Id == id);
            if (areaDetalle == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.Estado))
            {
                areaDetalle.Estado = dto.Estado;

                if (dto.Estado == "Finalizado")
                {
                    areaDetalle.Prioridad = 0;

                    var otrasAreas = await context.AreaDetalleOrdenes
                        .Where(a => a.OrdenId == areaDetalle.OrdenId && a.Id != areaDetalle.Id && a.Estado != "Finalizado")
                        .OrderBy(a => a.Prioridad)
                        .ToListAsync();

                    int nuevaPrioridad = 1;
                    foreach (var area in otrasAreas)
                    {
                        area.Prioridad = nuevaPrioridad;
                        nuevaPrioridad++;
                    }
                }
            }

            await context.SaveChangesAsync();

            var result = new AreaDetalleOrdenListaDTO
            {
                Id = areaDetalle.Id,
                OrdenId = areaDetalle.OrdenId,
                AreaId = areaDetalle.AreaId,
                Descripcion = areaDetalle.Descripcion,
                Estado = areaDetalle.Estado,
                Tiempo = areaDetalle.Tiempo,
                Comentario = areaDetalle.Comentario,
                Prioridad = areaDetalle.Prioridad,
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
                NombreArea = areaDetalle.Area?.NombreArea,
                NombreOrden = areaDetalle.Orden?.NombreOrden,
                NombreCliente = areaDetalle.Orden?.Cliente?.Nombre
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarArea(int id)
        {
            var areaDetalle = await context.AreaDetalleOrdenes.FindAsync(id);
            if (areaDetalle == null)
                return NotFound("Área detallada no encontrada");

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

        [HttpPut("{id}/Prioridad")]
        public async Task<IActionResult> ActualizarPrioridad(int id, [FromBody] int nuevaPrioridad)
        {
            var area = await context.AreaDetalleOrdenes.FindAsync(id);
            if (area == null)
                return NotFound("Área no encontrada.");

            area.Prioridad = nuevaPrioridad;
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
