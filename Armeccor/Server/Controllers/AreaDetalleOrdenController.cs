using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        //[HttpGet("{nroOT}")]
        //public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> GetAreasByNroOT(int nroOT)
        //{
        //    var orden = await context.Ordenes
        //        .Where(o => o.NroOT == nroOT)
        //        .FirstOrDefaultAsync();

        //    if (orden == null)
        //    {
        //        return NotFound($"No se encontró la orden con el número de OT: {nroOT}.");
        //    }

        //    var areasDetalle = await context.AreaDetalleOrdenes
        //        .Where(where => where.OrdenId == orden.Id)
        //        .Include(pe => pe.Area).Include(cliente=>cliente.Orden.Cliente)
        //        .Select(ado => new AreaDetalleOrdenListaDTO
        //        {
        //            Id = ado.Id,
        //            NombreOrden = orden.NombreOrden,
        //            NombreCliente = ado.Orden.Cliente.Nombre,
        //            NombreArea = ado.Area.NombreArea,
        //            Descripcion = ado.Descripcion,
        //            Estado = ado.Estado,
        //            Tiempo = ado.Tiempo,
        //            Comentario = ado.Comentario,
        //            Prioridad = ado.Prioridad
        //        })
        //        .ToListAsync();

        //    return Ok(areasDetalle);
        //}

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

        //[HttpPost("AreaDetallaEnOrden")]
        //public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        //{
        //    // ✅ Si viene NroOT, buscar el OrdenId
        //    if (dto.NroOT.HasValue)
        //    {
        //        var orden = await context.Ordenes
        //            .FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value);

        //        if (orden == null)
        //            return BadRequest($"No existe la orden con NroOT {dto.NroOT}");

        //        dto.OrdenId = orden.Id;
        //    }

        //    // ✅ Validar existencia de OrdenId y AreaId
        //    if (!await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId))
        //        return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");

        //    if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
        //        return BadRequest($"El Área con Id {dto.AreaId} no existe.");

        //    // ✅ Guardar entidad
        //    var entity = _mapper.Map<AreaDetalleOrden>(dto);
        //    context.AreaDetalleOrdenes.Add(entity);
        //    await context.SaveChangesAsync();

        //    // ✅ Incluir nombre del área en la respuesta
        //    var result = await context.AreaDetalleOrdenes
        //        .Include(x => x.Area)
        //        .FirstOrDefaultAsync(x => x.Id == entity.Id);

        //    var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
        //    resultDto.NombreArea = result.Area?.NombreArea;

        //    return Ok(resultDto);
        //}

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

            var entity = _mapper.Map<AreaDetalleOrden>(dto);
            entity.Prioridad = CalcularPrioridad(entity.Estado, entity.AreaId.Value);

            context.AreaDetalleOrdenes.Add(entity);
            await context.SaveChangesAsync();

            var result = await context.AreaDetalleOrdenes
                .Include(x => x.Area)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
            resultDto.NombreArea = result.Area?.NombreArea;

            return Ok(resultDto);
        }


        private int CalcularPrioridad(string estado, int areaId)
        {
            return estado switch
            {
                "Iniciado" => 1 + ((areaId - 1) % 3),     // 1–3
                "Pendiente" => 4 + ((areaId - 1) % 3),    // 4–6
                "Detenido" => 7 + ((areaId - 1) % 4),     // 7–10
                "Finalizado" => 0,
                _ => 0
            };
        }


        //[HttpPut("{id}/Estado")]
        //public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        //{
        //    var areaDetalle = await context.AreaDetalleOrdenes.Include(a => a.Area).FirstOrDefaultAsync(x => x.Id == id);
        //    if (areaDetalle == null) return NotFound();

        //    if (!string.IsNullOrEmpty(dto.Estado))
        //        areaDetalle.Estado = dto.Estado;

        //    await context.SaveChangesAsync();

        //    var result = new AreaDetalleOrdenListaDTO
        //    {
        //        Id = areaDetalle.Id,
        //        OrdenId = areaDetalle.OrdenId,
        //        AreaId = areaDetalle.AreaId,
        //        Descripcion = areaDetalle.Descripcion,
        //        Estado = areaDetalle.Estado,
        //        Tiempo = areaDetalle.Tiempo,
        //        NombreArea = areaDetalle.Area?.NombreArea
        //    };

        //    return Ok(result);
        //}

        [HttpPut("{id}/Estado")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        {
            var areaDetalle = await context.AreaDetalleOrdenes.Include(a => a.Area).FirstOrDefaultAsync(x => x.Id == id);
            if (areaDetalle == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.Estado))
            {
                areaDetalle.Estado = dto.Estado;
                areaDetalle.Prioridad = CalcularPrioridad(dto.Estado, areaDetalle.AreaId.Value);
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
