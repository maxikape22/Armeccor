using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Ordenes")]
    public class OrdenesController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public OrdenesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        // --- Método PUT para actualizar una Orden por ID ---
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutOrden(int id, CrearOrdenDTO ordenActualizacionDto)
        {
            var ordenExistente = await context.Ordenes.FindAsync(id);

            if (ordenExistente == null)
            {
                return NotFound($"No se pudo encontrar la orden con ID: {id}");
            }

            _mapper.Map(ordenActualizacionDto, ordenExistente);
            context.Entry(ordenExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Ordenes.AnyAsync(e => e.Id == id))
                {
                    return NotFound($"No se pudo actualizar el registro de Id: {id}. Posiblemente fue borrado.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes()
        //{
        //    var ordenes = await context.Ordenes
        //        .Include(o => o.Cliente)
        //        .ThenInclude(ao => ao.Ordenes)
        //        .Include(o => o.Plano)
        //        .Include(o => o.Entregas)
        //        .Include(a => a.AreaDetalleOrdenes)
        //        .ThenInclude(ad => ad.Area)
        //        .ToListAsync();

        //    return Ok(_mapper.Map<IEnumerable<OrdenDetalleDTO>>(ordenes));
        //}

        //Para hacer funcionar Signlr

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes()
        //{
        //    var ordenes = await context.Ordenes
        //        .Include(o => o.Cliente)
        //        .Include(o => o.Plano)
        //        .Include(o => o.Entregas)
        //        .Include(o => o.AreaDetalleOrdenes)
        //            .ThenInclude(ad => ad.Area)
        //        .ToListAsync();

        //    var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

        //    // rellenar AreaActual por NroOT => la area con Estado == "Iniciado" (si existe)
        //    foreach (var dto in ordenesDto)
        //    {
        //        var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);
        //        var areaActual = entidad?.AreaDetalleOrdenes?.FirstOrDefault(ad =>
        //            ad.Estado != null && ad.Estado.Equals("Iniciado", System.StringComparison.OrdinalIgnoreCase));
        //        dto.AreaActual = areaActual != null ? areaActual.Area?.NombreArea ?? "Sin área activa" : "Sin área activa";
        //    }

        //    return Ok(ordenesDto);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes()
        {
            var ordenes = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area)
                .ToListAsync();

            var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

            foreach (var dto in ordenesDto)
            {
                var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);

                // Buscar el área con estado "Iniciado"
                var areaActual = entidad?.AreaDetalleOrdenes?
                    .FirstOrDefault(ad =>
                        ad.Estado != null &&
                        ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));

                // Si hay un área en estado Iniciado, mostrar Nombre + Estado
                if (areaActual != null)
                {
                    var nombre = areaActual.Area?.NombreArea ?? "(Área sin nombre)";
                    dto.AreaActual = $"{nombre} ({areaActual.Estado})";
                }
                else
                {
                    dto.AreaActual = "No hay área cargada con estado incial";
                }
            }

            return Ok(ordenesDto);
        }

        [HttpGet("Original")]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrdenesOriginal()
        {
            var ordenes = await context.Ordenes
                .Include(x=>x.AreaDetalleOrdenes)
                .ThenInclude(y=>y.Area)
                .ToListAsync();
            return Ok(ordenes);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrdenDetalleDTO>> GetOrden(int id)
        {
            var orden = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orden is null)
            {
                return NotFound($"No se encontró el registro solicitado de Id: {id}");
            }
            return _mapper.Map<OrdenDetalleDTO>(orden);
        }

        [HttpPost]
        public async Task<ActionResult<CrearOrdenDTO>> PostOrden(CrearOrdenDTO crearOrdenDTO)
        {
            var clienteExiste = await context.Clientes.AnyAsync(c => c.Id == crearOrdenDTO.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest($"El Cliente con ID {crearOrdenDTO.ClienteId} no existe.");
            }

            var orden = _mapper.Map<Orden>(crearOrdenDTO);

            context.Ordenes.Add(orden);

            await context.SaveChangesAsync();

            var ordenDTO = _mapper.Map<CrearOrdenDTO>(orden);
            return Ok(ordenDTO);
        }

        [HttpPost("Original")]
        public async Task<ActionResult<Orden>> PostOrdenOriginal(Orden orden)
        {
            context.Ordenes.Add(orden);
            await context.SaveChangesAsync();
            return Ok(orden);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteOrden(int id)
        {
            var orden = await context.Ordenes.FindAsync(id);
            if (orden == null)
            {
                return NotFound($"No se pudo borrar el registro de Id: {id}. No encontrado.");
            }
            context.Ordenes.Remove(orden);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("OrdenConAreas")]
        public async Task<ActionResult<Orden>> GetOrdenConAreas(int id)
        {
            var orden = await context.Ordenes
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area) // 🔹 Para que traiga el NombreArea
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
                return NotFound();

            return Ok(orden);
        }

        //[HttpGet("{ordenId}/Areas")]
        //public async Task<ActionResult<List<object>>> GetAreasDeOrden(int ordenId)
        //{
        //    var orden = await context.Ordenes
        //        .Include(o => o.AreaDetalleOrdenes)
        //        .ThenInclude(ado => ado.Area)
        //        .FirstOrDefaultAsync(o => o.Id == ordenId);

        //    if (orden == null)
        //        return NotFound($"No se encontró la orden con ID {ordenId}");

        //    var result = orden.AreaDetalleOrdenes
        //        .Select(ado => new
        //        {
        //            ado.Id,
        //            ado.OrdenId,
        //            ado.AreaId,
        //            NombreArea = ado.Area.NombreArea,
        //            ado.Descripcion,
        //            ado.Estado,
        //            ado.Tiempo
        //        }).ToList();

        //    return Ok(result);
        //}

        //[HttpGet("{ordenId}/AreaActual")]
        //public async Task<ActionResult<object>> GetAreaActual(int ordenId)
        //{
        //    var orden = await context.Ordenes
        //        .Include(o => o.AreaDetalleOrdenes)
        //        .ThenInclude(ado => ado.Area)
        //        .FirstOrDefaultAsync(o => o.Id == ordenId);

        //    if (orden == null)
        //        return NotFound($"No se encontró la orden con ID {ordenId}");

        //    // regla: el área actual es la última con estado distinto de "Terminada"
        //    var areaActual = orden.AreaDetalleOrdenes
        //        .OrderByDescending(ado => ado.Id)
        //        .FirstOrDefault(ado => ado.Estado != "Terminada");

        //    if (areaActual == null)
        //        return NotFound("La orden no tiene un área actual asignada.");

        //    return Ok(new
        //    {
        //        areaActual.OrdenId,
        //        areaActual.AreaId,
        //        NombreArea = areaActual.Area.NombreArea,
        //        areaActual.Estado,
        //        areaActual.Tiempo
        //    });
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<OrdenDetalleDTO>> GetAreaOrden(int id)
        //{
        //    var orden = await context.Ordenes
        //        .Include(o => o.Cliente)
        //        .Include(o => o.AreaDetalleOrdenes)
        //        .FirstOrDefaultAsync(o => o.Id == id);

        //    if (orden == null)
        //        return NotFound();

        //    return _mapper.Map<OrdenDetalleDTO>(orden);
        //}

    }
}