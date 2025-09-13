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

        //Para hacer que el cambio de estado asigne la fecha de entrega al momento de cambiar de estado

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutOrden(int id, CrearOrdenDTO ordenActualizacionDto)
        {
            var ordenExistente = await context.Ordenes.FindAsync(id);

            if (ordenExistente == null)
                return NotFound($"No se pudo encontrar la orden con ID: {id}");

            // Mapear los cambios del DTO a la entidad
            _mapper.Map(ordenActualizacionDto, ordenExistente);

            // ✅ Si el nuevo estado es Finalizado y aún no tiene fecha, la asignamos
            if (ordenActualizacionDto.Estado.Equals("Finalizada", StringComparison.OrdinalIgnoreCase) &&
                !ordenExistente.FechaEntrega.HasValue)
            {
                DateTime time = DateTime.Now;
                var contador = time.AddDays(2);
                ordenExistente.FechaEntrega = DateTime.Now; //contador; //DateTime.Now;
            }

            context.Entry(ordenExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(); // ✅ se guarda todo junto
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Ordenes.AnyAsync(e => e.Id == id))
                    return NotFound($"No se pudo actualizar la orden Nro: {ordenExistente.NroOT}.");
                else
                    throw;
            }

            return NoContent();
        }

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
    }
}