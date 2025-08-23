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
            var InsumoDetalleOrdenListaDTO = await context.InsumoDetalleOrdenes
                .Select(x => new InsumoDetalleOrdenListaDTO { Id = x.Id, InsumoId = x.InsumoId, OrdenId = x.OrdenId, Cantidad = x.Cantidad })
                .ToListAsync();
            await context.InsumoDetalleOrdenes.ToListAsync();
            return Ok(InsumoDetalleOrdenListaDTO);
        }

        [HttpPost]
        public async Task<ActionResult<InsumoDetalleOrdenDTO>> PostCliente(InsumoDetalleOrdenDTO InsumoDetalleOrdenDTO)
        {
            var InsumoDetalleOrden = _mapper.Map<InsumoDetalleOrden>(InsumoDetalleOrdenDTO);
            context.InsumoDetalleOrdenes.Add(InsumoDetalleOrden);
            await context.SaveChangesAsync();
            var insumodetalleordenDTO = _mapper.Map<InsumoDetalleOrdenDTO>(InsumoDetalleOrden);
            return Ok(insumodetalleordenDTO);
        }
    }
}
