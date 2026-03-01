using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var orden = await context.Ordenes.Where(d=>d.EstaActivo).FirstOrDefaultAsync(o => o.NroOT == nroOT);
            if (orden == null) return NotFound($"No se encontró la orden con el número de OT: {nroOT}.");

            var areasDetalle = await context.AreaDetalleOrdenes
                .Where(e=>e.EstaActivo)
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
                    EstadoOrden = orden.Estado,
                    Tiempo = a.Tiempo,
                    Comentario = a.Comentario,
                    Prioridad = a.Estado == "Finalizado" ? 0 : a.Prioridad,
                    EstaActivo = a.EstaActivo,
                    FechaBaja = a.FechaBaja,
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
                .Where(e=>e.EstaActivo)
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
                    Prioridad = a.Estado == "Finalizado" ? 0 : a.Prioridad,
                    EstaActivo = a.EstaActivo,
                    FechaBaja = a.FechaBaja,

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

        //[HttpPost("AreaDetallaEnOrden")]
        //public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        //{
        //    if (dto.NroOT.HasValue)
        //    {
        //        var orden = await context.Ordenes.FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value);
        //        if (orden == null) return BadRequest($"No existe la orden con NroOT {dto.NroOT}");
        //        dto.OrdenId = orden.Id;
        //    }

        //    if (!await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId))
        //        return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");

        //    if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
        //        return BadRequest($"El Área con Id {dto.AreaId} no existe.");

        //    // 🚫 Solo bloquear si el nuevo estado es "Iniciado"
        //    if (dto.Estado == "Iniciado")
        //    {
        //        bool yaExisteIniciado = await context.AreaDetalleOrdenes
        //            .AnyAsync(a => a.OrdenId == dto.OrdenId && a.AreaId == dto.AreaId && a.Estado == "Iniciado" && a.EstaActivo);

        //        if (yaExisteIniciado)
        //            return BadRequest("Ya existe un registro de esta área con estado 'Iniciado' en la misma orden.");
        //    }

        //    // ✅ Calcular prioridad defensiva (excluye finalizados y eliminados)
        //    int prioridad = await context.AreaDetalleOrdenes
        //        .CountAsync(a => a.OrdenId == dto.OrdenId && a.EstaActivo && a.Estado != "Finalizado") + 1;

        //    var entity = _mapper.Map<AreaDetalleOrden>(dto);
        //    entity.Prioridad = dto.Estado == "Finalizado" ? 0 : prioridad;
        //    entity.EstaActivo = true;
        //    entity.FechaBaja = null;

        //    context.AreaDetalleOrdenes.Add(entity);
        //    await context.SaveChangesAsync();

        //    // 🧠 Reasignar prioridad disponible
        //    await context.Database.ExecuteSqlRawAsync(
        //        "EXEC sp_AsignarPrioridadDisponible @p0, @p1",
        //        parameters: new[] { dto.OrdenId.ToString(), entity.Id.ToString() }
        //    );


        //    var result = await context.AreaDetalleOrdenes
        //        .Include(x => x.Area)
        //        .FirstOrDefaultAsync(x => x.Id == entity.Id);

        //    var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
        //    resultDto.NombreArea = result.Area?.NombreArea;

        //    return Ok(resultDto);
        //}

        //[HttpPost("AreaDetallaEnOrden")]
        //public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        //{
        //    if (dto.NroOT.HasValue)
        //    {
        //        var orden = await context.Ordenes.FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value);
        //        if (orden == null) return BadRequest($"No existe la orden con NroOT {dto.NroOT}");
        //        dto.OrdenId = orden.Id;
        //    }

        //    if (!await context.Ordenes.AnyAsync(o => o.Id == dto.OrdenId))
        //        return BadRequest($"La Orden con Id {dto.OrdenId} no existe.");

        //    if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
        //        return BadRequest($"El Área con Id {dto.AreaId} no existe.");

        //    // 🚫 Bloqueo defensivo
        //    if (dto.Estado == "Iniciado")
        //    {
        //        bool yaExisteIniciado = await context.AreaDetalleOrdenes
        //            .AnyAsync(a => a.OrdenId == dto.OrdenId && a.AreaId == dto.AreaId && a.Estado == "Iniciado" && a.EstaActivo);

        //        if (yaExisteIniciado)
        //            return BadRequest("Ya existe un registro de esta área con estado 'Iniciado' en la misma orden.");
        //    }

        //    // 🔧 Crear entidad con prioridad provisional (0)
        //    var entity = _mapper.Map<AreaDetalleOrden>(dto);
        //    entity.Prioridad = 0; // se asignará en el SP
        //    entity.EstaActivo = true;
        //    entity.FechaBaja = null;

        //    context.AreaDetalleOrdenes.Add(entity);
        //    await context.SaveChangesAsync();

        //    // ✅ Llamar al procedimiento almacenado para asignar prioridad disponible
        //    await context.Database.ExecuteSqlRawAsync(
        //        "EXEC sp_AsignarPrioridadDisponible @p0, @p1",
        //        parameters: new object[] { dto.OrdenId, entity.Id }
        //    );

        //    // 🔁 Traer el resultado actualizado
        //    var result = await context.AreaDetalleOrdenes
        //        .Include(x => x.Area)
        //        .FirstOrDefaultAsync(x => x.Id == entity.Id);

        //    var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
        //    resultDto.NombreArea = result.Area?.NombreArea;

        //    return Ok(resultDto);
        //}


        //[HttpPost("AreaDetallaEnOrden")]
        //public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        //{
        //    if (dto.NroOT.HasValue)
        //    {
        //        var ordenPorNro = await context.Ordenes
        //            .FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value && o.EstaActivo);

        //        if (ordenPorNro == null)
        //            return BadRequest($"No existe la orden con NroOT {dto.NroOT}");

        //        dto.OrdenId = ordenPorNro.Id;
        //    }

        //    var orden = await context.Ordenes
        //        .FirstOrDefaultAsync(o => o.Id == dto.OrdenId && o.EstaActivo);

        //    if (orden == null)
        //        return BadRequest($"La Orden con Id {dto.OrdenId} no existe o está dada de baja.");

        //    // 🚫 REGLA DE NEGOCIO NUEVA
        //    if (!orden.Estado.Equals("Iniciada", StringComparison.OrdinalIgnoreCase) &&
        //        !orden.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return BadRequest(
        //            $"No se pueden agregar áreas a una orden en estado '{orden.Estado}'. " +
        //            "Solo se permite el estado Iniciada o Pendiente para el registro del área en la orden."
        //        );
        //    }

        //    if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
        //        return BadRequest($"El Área con Id {dto.AreaId} no existe.");

        //    // 🚫 Bloqueo defensivo existente
        //    if (dto.Estado == "Iniciado")
        //    {
        //        bool yaExisteIniciado = await context.AreaDetalleOrdenes
        //            .AnyAsync(a => a.OrdenId == dto.OrdenId &&
        //                           a.AreaId == dto.AreaId &&
        //                           a.Estado == "Iniciado" &&
        //                           a.EstaActivo);

        //        if (yaExisteIniciado)
        //            return BadRequest("Ya existe un registro de esta área con estado 'Iniciado' en la misma orden.");
        //    }

        //    var entity = _mapper.Map<AreaDetalleOrden>(dto);
        //    entity.Prioridad = 0;
        //    entity.EstaActivo = true;
        //    entity.FechaBaja = null;

        //    context.AreaDetalleOrdenes.Add(entity);
        //    await context.SaveChangesAsync();

        //    await context.Database.ExecuteSqlRawAsync(
        //        "EXEC sp_AsignarPrioridadDisponible @p0, @p1",
        //        new object[] { dto.OrdenId, entity.Id }
        //    );

        //    var result = await context.AreaDetalleOrdenes
        //        .Include(x => x.Area)
        //        .FirstOrDefaultAsync(x => x.Id == entity.Id);

        //    var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
        //    resultDto.NombreArea = result.Area?.NombreArea;

        //    return Ok(resultDto);
        //}

        //ADAPTACION DEL POST ANTERIOR PARA VALIDAR EL ESTADO DE LA ORDEN ANTES DE PERMITIR AGREGAR ÁREAS, Y MANTENER LA REGLA DE NEGOCIO DE NO PERMITIR MÁS DE UN "INICIADO" POR ÁREA EN LA MISMA ORDEN. SE ASUME QUE SOLO SE PUEDEN AGREGAR ÁREAS A ÓRDENES EN ESTADO "Iniciada" O "Pendiente".

        [HttpPost("AreaDetallaEnOrden")]
        public async Task<ActionResult<AreaDetalleOrdenDTO>> PostAreaDetalleOrden(AreaDetalleOrdenDTO dto)
        {
            if (dto.NroOT.HasValue)
            {
                var ordenPorNro = await context.Ordenes
                    .FirstOrDefaultAsync(o => o.NroOT == dto.NroOT.Value && o.EstaActivo);

                if (ordenPorNro == null)
                    return BadRequest($"No existe la orden con NroOT {dto.NroOT}");

                dto.OrdenId = ordenPorNro.Id;
            }

            var orden = await context.Ordenes
                .FirstOrDefaultAsync(o => o.Id == dto.OrdenId && o.EstaActivo);

            if (orden == null)
                return BadRequest($"La Orden con Id {dto.OrdenId} no existe o está dada de baja.");

            if (!orden.Estado.Equals("Iniciada", StringComparison.OrdinalIgnoreCase) &&
                !orden.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(
                    $"No se pueden agregar áreas a una orden en estado '{orden.Estado}'. " +
                    "Solo se permite el estado Iniciada o Pendiente."
                );
            }

            if (!await context.Areas.AnyAsync(a => a.Id == dto.AreaId))
                return BadRequest($"El Área con Id {dto.AreaId} no existe.");

            // 🔴 REGLA FUERTE: SOLO UN INICIADO POR ORDEN
            if (dto.Estado == "Iniciado")
            {
                bool yaExisteIniciado = await context.AreaDetalleOrdenes
                    .AnyAsync(a =>
                        a.OrdenId == dto.OrdenId &&
                        a.EstaActivo &&
                        a.Estado == "Iniciado");

                if (yaExisteIniciado)
                    return BadRequest(
                        "Ya existe un área con estado 'Iniciado' en esta orden. " +
                        "Solo puede haber una área iniciada a la vez."
                    );
            }

            var entity = _mapper.Map<AreaDetalleOrden>(dto);
            entity.Prioridad = 0;
            entity.EstaActivo = true;
            entity.FechaBaja = null;

            context.AreaDetalleOrdenes.Add(entity);
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync(
                "EXEC sp_AsignarPrioridadDisponible @p0, @p1",
                new object[] { dto.OrdenId, entity.Id }
            );

            var result = await context.AreaDetalleOrdenes
                .Include(x => x.Area)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            var resultDto = _mapper.Map<AreaDetalleOrdenDTO>(result);
            resultDto.NombreArea = result.Area?.NombreArea;

            return Ok(resultDto);
        }


        //


        //[HttpPut("{id}/Estado")]
        //public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(int id, [FromBody] AreaDetalleOrdenListaDTO dto)
        //{
        //    var areaDetalle = await context.AreaDetalleOrdenes
        //        .Include(a => a.Area)
        //        .FirstOrDefaultAsync(x => x.Id == id);

        //    if (areaDetalle == null)
        //        return NotFound();

        //    if (!string.IsNullOrEmpty(dto.Estado))
        //    {
        //        areaDetalle.Estado = dto.Estado;

        //        // ✅ Si se marca como Finalizado, se asigna prioridad 0
        //        if (dto.Estado == "Finalizado")
        //        {
        //            areaDetalle.Prioridad = 0;

        //            // 🔁 Reordenar prioridades de áreas activas y no finalizadas
        //            var otrasAreas = await context.AreaDetalleOrdenes
        //                .Where(a => a.OrdenId == areaDetalle.OrdenId &&
        //                            a.Id != areaDetalle.Id &&
        //                            a.EstaActivo && // 🔒 excluye eliminadas
        //                            a.Estado != "Finalizado")
        //                .OrderBy(a => a.Prioridad)
        //                .ToListAsync();

        //            int nuevaPrioridad = 1;
        //            foreach (var area in otrasAreas)
        //            {
        //                area.Prioridad = nuevaPrioridad++;
        //            }
        //        }
        //    }

        //    await context.SaveChangesAsync();

        //    var result = new AreaDetalleOrdenListaDTO
        //    {
        //        Id = areaDetalle.Id,
        //        OrdenId = areaDetalle.OrdenId,
        //        AreaId = areaDetalle.AreaId,
        //        Descripcion = areaDetalle.Descripcion,
        //        Estado = areaDetalle.Estado,
        //        Tiempo = areaDetalle.Tiempo,
        //        Comentario = areaDetalle.Comentario,
        //        Prioridad = areaDetalle.Prioridad,
        //        NombreArea = areaDetalle.Area?.NombreArea
        //    };

        //    return Ok(result);
        //}

        //ADAPTACION DEL PUT ANTERIOR PARA QUE AL CAMBIAR EL ESTADO A "Finalizado" SE ASIGNE PRIORIDAD 0, PERMITIENDO MANTENER EL ORDEN DE PRIORIDAD DE LAS ÁREAS ACTIVAS Y NO FINALIZADAS, SIN INCLUIR LAS ÁREAS ELIMINADAS (BAJA LÓGICA) EN LA REORDENACIÓN. SE ASUME QUE SOLO LAS ÁREAS ACTIVAS Y NO FINALIZADAS PARTICIPAN EN LA LÓGICA DE PRIORIDAD, MIENTRAS QUE LAS ÁREAS FINALIZADAS O ELIMINADAS QUEDAN FUERA DE ELLA.


        [HttpPut("{id}/Estado")]
        public async Task<ActionResult<AreaDetalleOrdenListaDTO>> CambiarEstado(
    int id,
    [FromBody] AreaDetalleOrdenListaDTO dto)
        {
            var areaDetalle = await context.AreaDetalleOrdenes
                .Include(a => a.Area)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (areaDetalle == null)
                return NotFound();

            if (!string.IsNullOrEmpty(dto.Estado))
            {
                // 🔴 BLOQUEO: Solo un iniciado por orden
                if (dto.Estado == "Iniciado")
                {
                    bool yaExisteIniciado = await context.AreaDetalleOrdenes
                        .AnyAsync(a =>
                            a.OrdenId == areaDetalle.OrdenId &&
                            a.Id != areaDetalle.Id &&
                            a.EstaActivo &&
                            a.Estado == "Iniciado");

                    if (yaExisteIniciado)
                        return BadRequest(
                            "Ya existe un área con estado 'Iniciado' en esta orden. " +
                            "Solo puede haber una área iniciada a la vez."
                        );
                }

                areaDetalle.Estado = dto.Estado;

                if (dto.Estado == "Finalizado")
                {
                    areaDetalle.Prioridad = 0;

                    var otrasAreas = await context.AreaDetalleOrdenes
                        .Where(a =>
                            a.OrdenId == areaDetalle.OrdenId &&
                            a.Id != areaDetalle.Id &&
                            a.EstaActivo &&
                            a.Estado != "Finalizado")
                        .OrderBy(a => a.Prioridad)
                        .ToListAsync();

                    int nuevaPrioridad = 1;
                    foreach (var area in otrasAreas)
                    {
                        area.Prioridad = nuevaPrioridad++;
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

            return Ok("Estado actualizado correctamente");
        }

        //

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> EliminarArea(int id)
        //{
        //    var areadetallada = await context.AreaDetalleOrdenes.FirstOrDefaultAsync(e => e.Id == id);

        //    if (areadetallada == null)
        //        return NotFound("Área detallada no encontrada.");

        //    if (!areadetallada.EstaActivo)
        //        return BadRequest("El área detallada ya está dada de baja.");

        //    // 🔴 Baja lógica
        //    areadetallada.EstaActivo = false;
        //    areadetallada.FechaBaja = DateTime.Now;

        //    await context.SaveChangesAsync();

        //    // 🔁 Reordenar prioridades de las áreas activas restantes (excluyendo finalizadas)
        //    var areasActivas = await context.AreaDetalleOrdenes
        //        .Where(a => a.OrdenId == areadetallada.OrdenId && a.EstaActivo && a.Estado != "Finalizado")
        //        .OrderBy(a => a.Prioridad)
        //        .ToListAsync();

        //    int nuevaPrioridad = 1;
        //    foreach (var area in areasActivas)
        //    {
        //        area.Prioridad = nuevaPrioridad++;
        //    }

        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarArea(int id)
        {
            var areadetallada = await context.AreaDetalleOrdenes.FirstOrDefaultAsync(e => e.Id == id);

            if (areadetallada == null)
                return NotFound("Área detallada no encontrada.");

            if (!areadetallada.EstaActivo)
                return BadRequest("El área detallada ya está dada de baja.");

            // 🔴 Baja lógica
            areadetallada.EstaActivo = false;
            areadetallada.FechaBaja = DateTime.Now;

            await context.SaveChangesAsync();

            // 🔁 Ajustar prioridades: las posteriores bajan una posición
            var areasPosteriores = await context.AreaDetalleOrdenes
                .Where(a => a.OrdenId == areadetallada.OrdenId
                            && a.EstaActivo
                            && a.Estado != "Finalizado"
                            && a.Prioridad > areadetallada.Prioridad)
                .ToListAsync();

            foreach (var area in areasPosteriores)
            {
                area.Prioridad -= 1;
            }

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


        [HttpPut("ReordenarPrioridad/{id}")]
        public async Task<ActionResult<List<AreaDetalleOrdenListaDTO>>> ReordenarPrioridad(int id, [FromBody] int nuevaPrioridad)
        {
            await context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.sp_ReordenarPrioridadArea @AreaId = {0}, @NuevaPrioridad = {1}",
                id, nuevaPrioridad);

            var ordenId = await context.AreaDetalleOrdenes
                .Where(x => x.Id == id)
                .Select(x => x.OrdenId)
                .FirstOrDefaultAsync();

            var dtoList = await context.AreaDetalleOrdenes
                .Where(a => a.OrdenId == ordenId && a.EstaActivo)
                .Include(a => a.Area)
                .Include(a => a.Orden).ThenInclude(o => o.Cliente)
                .Select(a => new AreaDetalleOrdenListaDTO
                {
                    Id = a.Id,
                    OrdenId = a.OrdenId,
                    AreaId = a.AreaId,
                    NombreOrden = a.Orden.NombreOrden,
                    NombreCliente = a.Orden.Cliente.Nombre,
                    NombreArea = a.Area.NombreArea,
                    Descripcion = a.Descripcion,
                    Estado = a.Estado,
                    Tiempo = a.Tiempo,
                    Comentario = a.Comentario,
                    Prioridad = a.Estado == "Finalizado" ? 0 : a.Prioridad,
                    EstaActivo = a.EstaActivo,
                    FechaBaja = a.FechaBaja
                })
                .OrderBy(a => a.Estado == "Finalizado" ? int.MaxValue : a.Prioridad)
                .ToListAsync();

            return Ok(dtoList);
        }


        //[HttpPut("{id}/Temporizador")]
        //[Consumes("application/json", "text/plain")]
        //public async Task<ActionResult> Temporizador(int id, [FromBody] string accion)
        //{
        //    if (string.IsNullOrWhiteSpace(accion))
        //        return BadRequest("Acción requerida.");

        //    accion = accion.ToUpperInvariant();
        //    if (accion != "INICIAR" && accion != "DETENER")
        //        return BadRequest("Acción inválida. Use INICIAR o DETENER.");

        //    // 1) Validación defensiva en EF contra AreaDetalleOrdenes
        //    var existe = await context.AreaDetalleOrdenes
        //        .AsNoTracking()
        //        .AnyAsync(a => a.Id == id);

        //    if (!existe)
        //        return NotFound($"ÁreaDetalleOrden con Id={id} no existe.");

        //    // 2) Ejecutar SP con parámetros interpolados (seguro)
        //    await context.Database.ExecuteSqlInterpolatedAsync(
        //        $"EXEC dbo.sp_TemporizadorArea {id}, {accion}"
        //    );

        //    return NoContent();
        //}
        //

        //[HttpPut("{id}/Temporizador")]
        //public async Task<ActionResult> Temporizador(int id, [FromBody] string accion)
        //{
        //    if (string.IsNullOrWhiteSpace(accion))
        //        return BadRequest("Acción requerida.");

        //    accion = accion.ToUpperInvariant();
        //    if (accion != "INICIAR" && accion != "DETENER")
        //        return BadRequest("Acción inválida. Use INICIAR o DETENER.");

        //    var stopwatch = Stopwatch.StartNew();

        //    await context.Database.ExecuteSqlInterpolatedAsync(
        //        $"EXEC dbo.sp_TemporizadorArea {id}, {accion}"
        //    );

        //    stopwatch.Stop();

        //    return Ok(new
        //    {
        //        Mensaje = "Acción ejecutada correctamente",
        //        DuracionMs = stopwatch.ElapsedMilliseconds
        //    });
        //}
        //

        public class TemporizadorRequest
        {
            public string Accion { get; set; } = "";
            public int? Delta { get; set; } // opcional
        }

        [HttpPut("{id}/Temporizador")]
        public async Task<ActionResult> Temporizador(int id, [FromBody] TemporizadorRequest req)
        {
            if (req is null || string.IsNullOrWhiteSpace(req.Accion))
                return BadRequest("Acción requerida.");

            var accion = req.Accion.ToUpperInvariant();
            if (accion != "INICIAR" && accion != "DETENER")
                return BadRequest("Acción inválida.");

            var delta = req.Delta ?? 1;
            if (delta <= 0) delta = 1;

            // ✅ Llamada al nuevo procedimiento
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC dbo.sp_TemporizadorRegresivoSegundos {id}, {accion}, {delta}"
            );

            return NoContent();
        }


    }
}
