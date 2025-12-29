using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    [Route("api/PedidoDetalleInsumos")]
    public class PedidoDetalleInsumosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PedidoDetalleInsumosController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPatch("Detalle/{id:int}/Estado")]
        //    public async Task<ActionResult> PatchEstadoPedidoDetalleInsumo(
        //int id,
        //[FromBody] string nuevoEstado)
        //    {
        //        var detalle = await _context.PedidoDetalleInsumos
        //            .FirstOrDefaultAsync(d => d.Id == id);

        //        if (detalle == null)
        //            return NotFound($"Detalle de pedido no encontrado (ID: {id})");

        //        // 🟢 cambio puntual
        //        detalle.Estado = nuevoEstado;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }

        //        return NoContent();
        //    }
        public async Task<IActionResult> CambiarEstadoPedidoDetalle(
    int id,
    [FromBody] string nuevoEstado)
        {
            var detalle = await _context.PedidoDetalleInsumos
                .FirstOrDefaultAsync(x => x.Id == id);

            if (detalle == null)
                return NotFound("Detalle de pedido no encontrado.");

            detalle.Estado = nuevoEstado;

            // 🔁 Lógica automática según estado
            switch (nuevoEstado)
            {
                case "Pendiente":
                case "Solicitado":
                    detalle.EsSolicitado = false;
                    detalle.EntregaParcial = false;
                    detalle.EntregaTotal = false;
                    break;

                case "Recibido":
                    detalle.EsSolicitado = true;
                    detalle.EntregaParcial = false;
                    detalle.EntregaTotal = false;
                    break;

                case "Entrega parcial":
                    detalle.EsSolicitado = true;
                    detalle.EntregaParcial = true;
                    detalle.EntregaTotal = false;
                    break;

                case "Entrega total":
                    detalle.EsSolicitado = true;
                    detalle.EntregaParcial = false;
                    detalle.EntregaTotal = true;
                    break;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }




        [HttpPatch("{id}/ActualizarSolicitud")]
        public async Task<IActionResult> ActualizarSolicitud(int id, [FromBody] bool nuevoEstado)
        {
            var detalle = await _context.PedidoDetalleInsumos.FindAsync(id);
            if (detalle == null)
                return NotFound($"No se encontró el detalle con ID {id}");

            detalle.EsSolicitado = nuevoEstado;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // o return Ok(detalle) si querés devolver el objeto actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado: {ex.Message}");
            }
        }

        [HttpPatch("{id}/ActualizarEntregaParcial")]
        public async Task<IActionResult> ActualizarEntregaParcial(int id, [FromBody] bool nuevoEstado)
        {
            var detalle = await _context.PedidoDetalleInsumos.FindAsync(id);
            if (detalle == null)
                return NotFound($"No se encontró el detalle con ID {id}");
            detalle.EntregaParcial = nuevoEstado;
            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // o return Ok(detalle) si querés devolver el objeto actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado: {ex.Message}");
            }
        }

        [HttpPatch("{id}/ActualizarEntregaTotal")]
        public async Task<IActionResult> ActualizarEntregaTotal(int id, [FromBody] bool nuevoEstado)
        {
            var detalle = await _context.PedidoDetalleInsumos.FindAsync(id);
            if (detalle == null)
                return NotFound($"No se encontró el detalle con ID {id}");
            detalle.EntregaTotal = nuevoEstado;
            try
            {
                await _context.SaveChangesAsync();
                return NoContent(); // o return Ok(detalle) si querés devolver el objeto actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado: {ex.Message}");
            }
        }

        // ✅ GET: Listar detalles de un pedido
        [HttpGet("{idPedido}")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleInsumoDTO>>> GetDetallesPorPedido(int idPedido)
        {
            var detalles = await _context.PedidoDetalleInsumos
                .Include(p => p.Insumo)
                    .ThenInclude(i => i.InsumoOrdenes)
                        .ThenInclude(io => io.Orden)
                .Include(p => p.Pedido)
                    .ThenInclude(pe => pe.Proveedor)
                .Where(p => p.IdPedido == idPedido)
                .ProjectTo<PedidoDetalleInsumoDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(detalles);
        }       

        // ✅ PATCH: Actualizar solo la cantidad
        [HttpPatch("{id}/ActualizarCantidad")]
        public async Task<IActionResult> ActualizarCantidad(int id, [FromBody] int nuevaCantidad)
        {
            var detalle = await _context.PedidoDetalleInsumos.FindAsync(id);
            if (detalle == null)
                return NotFound("Detalle no encontrado");

            detalle.Cantidad = nuevaCantidad;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Cantidad actualizada correctamente" });
        }


        [HttpGet("ListarDetalleInsumos")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleInsumoDTO>>> GetDetalleInsumos()
        {
            var detalles = await _context.PedidoDetalleInsumos              
                .Include(p => p.Insumo)
                    .ThenInclude(i => i.InsumoOrdenes)
                    .ThenInclude(io => io.Orden)          
                    .Include(p => p.Pedido)
                    .ThenInclude(pe => pe.Proveedor)
                    .ProjectTo<PedidoDetalleInsumoDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();
            return Ok(detalles);
        }


        // ✅ GET: api/PedidoDetalleInsumo/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoDetalleInsumoDTO>> GetById(int id)
        {
            var entidad = await _context.PedidoDetalleInsumos.FindAsync(id);
            if (entidad == null)
                return NotFound();

            var dto = _mapper.Map<PedidoDetalleInsumoDTO>(entidad);
            return Ok(dto);
        }

        [HttpPost("CrearDetalleInsumo")]
        public async Task<ActionResult<PedidoDetalleInsumoDTO>> PostDetalleInsumo(PedidoDetalleInsumoDTO dto)
        {
            // 🔍 Validar existencia de Pedido
            var pedido = await _context.Pedidos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == dto.IdPedido);

            if (pedido == null)
                return BadRequest($"No existe el pedido con ID {dto.IdPedido}");

            // 🔍 Validar existencia de Insumo
            var insumo = await _context.Insumos
                .Include(i => i.InsumoOrdenes)
                    .ThenInclude(io => io.Orden)
                .FirstOrDefaultAsync(i => i.Id == dto.IdInsumo);

            if (insumo == null)
                return BadRequest($"No existe el insumo con ID {dto.IdInsumo}");

            // 🔁 Mapear entidad
            var entidad = _mapper.Map<PedidoDetalleInsumo>(dto);
            entidad.Pedido = pedido;
            entidad.Insumo = insumo;

            _context.PedidoDetalleInsumos.Add(entidad);
            await _context.SaveChangesAsync();

            // 🔁 Obtener entidad completa para respuesta
            var result = await _context.PedidoDetalleInsumos
                .Include(x => x.Pedido)
                    .ThenInclude(p => p.Proveedor)
                .Include(x => x.Insumo)
                    .ThenInclude(i => i.InsumoOrdenes)
                        .ThenInclude(io => io.Orden)
                .FirstOrDefaultAsync(x => x.Id == entidad.Id);

            var resultDto = _mapper.Map<PedidoDetalleInsumoDTO>(result);
            //resultDto.NombreProveedor = result.Pedido?.Proveedor?.Nombre;
            resultDto.NroOT = result.Insumo?.InsumoOrdenes.FirstOrDefault()?.Orden?.NroOT ?? 0;

            return Ok(resultDto);
        }


        [HttpGet("ListarInsumosInsuficientes")]
        public async Task<ActionResult<IEnumerable<PedidoDetalleInsumoDTO>>> GetInsumosInsuficientes()
        {
            var lista = await _context.Insumos
                .Where(i => i.CantDisponible <= 0)
                .Select(i => new PedidoDetalleInsumoDTO
                {
                    IdInsumo = i.Id,
                    Item = i.Nombre,
                    Cantidad = (int)i.CantDisponible,
                    FechaUso = DateTime.Now,
                    Estado = "Pendiente"
                })
                .ToListAsync();

            return Ok(lista);
        }

    }
}
