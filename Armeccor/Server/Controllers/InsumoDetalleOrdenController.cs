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
    [Route("api/Insumo_Detalle_Orden")]
    public class InsumoDetalleOrdenController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public InsumoDetalleOrdenController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        // ✅ Obtener todos los detalles
        [HttpGet]
        public async Task<ActionResult<List<InsumoDetalleOrdenListaDTO>>> Get()
        {
            var lista = await context.InsumoDetalleOrdenes.ToListAsync();
            return Ok(lista);
        }

        // ✅ Agregar o actualizar insumo en orden (suma cantidades)
        [HttpPost]
        public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        {
            // Verificar existencia del insumo
            var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo == null)
                return NotFound($"No se encontró el insumo con Id {dto.InsumoId}");

            // Verificar existencia de la orden
            var orden = await context.Ordenes.FirstOrDefaultAsync(x => x.Id == dto.OrdenId);
            if (orden == null)
                return NotFound($"No se encontró la orden con Id {dto.OrdenId}");

            // Buscar si ya existe un detalle para ese insumo y orden
            var detalleExistente = await context.InsumoDetalleOrdenes
                .FirstOrDefaultAsync(x => x.OrdenId == dto.OrdenId && x.InsumoId == dto.InsumoId);

            if (detalleExistente != null)
            {
                // 🔹 Si ya existe → sumar cantidad
                detalleExistente.Cantidad += dto.Cantidad;
                context.InsumoDetalleOrdenes.Update(detalleExistente);
            }
            else
            {
                // 🔹 Si no existe → crear nuevo detalle
                var nuevoDetalle = new InsumoDetalleOrden
                {
                    OrdenId = dto.OrdenId,
                    InsumoId = dto.InsumoId,
                    Cantidad = dto.Cantidad
                };
                context.InsumoDetalleOrdenes.Add(nuevoDetalle);
            }

            // 🔹 Actualizar stock (aunque quede negativo)
            insumo.CantDisponible -= dto.Cantidad;

            await context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"Se agregó {dto.Cantidad} de {insumo.Nombre} a la orden {dto.OrdenId}.",
                InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo)
            });
        }

        [HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        public async Task<ActionResult> GetInsumosAgrupadosPorOrden(int ordenId)
        {
            var query = await context.InsumoDetalleOrdenes
                .Include(x => x.Insumo)
                .Where(x => x.OrdenId == ordenId)
                .GroupBy(x => new { x.InsumoId, x.Insumo.Nombre })
                .Select(g => new InsumoDetalleOrdenDTO
                {
                    InsumoId = g.Key.InsumoId.Value,
                    Nombre = g.Key.Nombre,
                    Cantidad = g.Sum(x => x.Cantidad), // 👈 suma positiva o negativa
                    OrdenId = ordenId
                })
                .ToListAsync();

            return Ok(query);
        }
        //qmetodo para liberar los insumos que tienen cargados una orden ,
        [HttpPost("Liberar")]
        public async Task<ActionResult> LiberarInsumo(LiberarInsumoDTO dto)
        {
            // Buscar el detalle de la orden para este insumo
            var detalle = await context.InsumoDetalleOrdenes
                .FirstOrDefaultAsync(x => x.OrdenId == dto.OrdenId && x.InsumoId == dto.InsumoId);

            if (detalle == null)
                return NotFound("No se encontró el insumo en la orden.");

            // Validar que la cantidad a liberar sea positiva
            if (dto.Cantidad <= 0)
                return BadRequest("Cantidad inválida para liberar.");

            // Actualizar el detalle de la orden: restar lo liberado
            detalle.Cantidad -= dto.Cantidad;

            // Actualizar el stock general de insumos: sumar lo liberado
            var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo != null)
            {
                insumo.CantDisponible += dto.Cantidad; // Puede quedar positivo o negativo
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"Se liberaron {dto.Cantidad} de {insumo?.Nombre} de la orden {dto.OrdenId}.",
                InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo),
                DetalleActualizado = _mapper.Map<InsumoDetalleOrdenDTO>(detalle)
            });
        }

        // DTO para liberar insumos
        public class LiberarInsumoDTO
        {
            public int OrdenId { get; set; }
            public int InsumoId { get; set; }
            public int Cantidad { get; set; }
        }

    }
    }
