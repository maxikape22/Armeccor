using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Insumo_Detalle_Orden")]
    public class InsumoDetalleOrdenController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        public InsumoDetalleOrdenController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<InsumoDetalleOrdenListaDTO>> Get()
        {
            var InsumoDetalleOrdenListaDTO = await context.InsumoDetalleOrdenes.ToListAsync();
            await context.InsumoDetalleOrdenes.ToListAsync();
            return Ok(InsumoDetalleOrdenListaDTO);
        }

        //[HttpPost]
        //public async Task<ActionResult<InsumoDetalleOrdenDTO>> PostCliente(InsumoDetalleOrdenDTO InsumoDetalleOrdenDTO)
        //{
        //    var InsumoDetalleOrden = _mapper.Map<InsumoDetalleOrden>(InsumoDetalleOrdenDTO);
        //    context.InsumoDetalleOrdenes.Add(InsumoDetalleOrden);
        //    await context.SaveChangesAsync();
        //    var insumodetalleordenDTO = _mapper.Map<InsumoDetalleOrdenDTO>(InsumoDetalleOrden);
        //    return Ok(insumodetalleordenDTO);
        //}

        [HttpPost]
        public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        {
            // Verificar que exista el insumo
            var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo == null)
                return NotFound($"No se encontró el insumo con Id {dto.InsumoId}");

            // Verificar que exista la orden
            var orden = await context.Ordenes.FirstOrDefaultAsync(x => x.Id == dto.OrdenId);
            if (orden == null)
                return NotFound($"No se encontró la orden con Id {dto.OrdenId}");

            // Validar stock
            if (insumo.CantDisponible < dto.Cantidad)
                return BadRequest($"Stock insuficiente. Disponible: {insumo.CantDisponible}, solicitado: {dto.Cantidad}");

            // Restar stock
            insumo.CantDisponible -= dto.Cantidad;

            // Crear detalle
            var detalle = new InsumoDetalleOrden
            {
                OrdenId = dto.OrdenId,   // 👈 obligatorio
                InsumoId = dto.InsumoId,
                Cantidad = dto.Cantidad
            };

            context.InsumoDetalleOrdenes.Add(detalle);
            await context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"Se agregó {dto.Cantidad} de {insumo.Nombre} a la orden {dto.OrdenId}",
                InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo),
                Detalle = _mapper.Map<InsumoDetalleOrdenDTO>(detalle)
            });
        }
        //agrupar insumos de la orden
        [HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        public async Task<ActionResult> GetInsumosAgrupadosPorOrden(int ordenId)
        {
            var query = await context.InsumoDetalleOrdenes
    .Include(x => x.Insumo)
    .Where(x => x.OrdenId == ordenId)
    .GroupBy(x => new { x.InsumoId, x.Insumo.Nombre })
    .Select(g => new InsumoDetalleOrdenDTO
    {
        InsumoId = g.Key.InsumoId.Value,   // 👈 corrección
        Nombre = g.Key.Nombre,
        Cantidad = g.Sum(x => x.Cantidad),
        OrdenId = ordenId
    })
    .ToListAsync();


            return Ok(query);
        }


    }
}
