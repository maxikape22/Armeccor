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
    [Route("api/Pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public PedidosController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        // ✅ GET: Obtener estado + proveedor del pedido
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                return NotFound();

            var dto = _mapper.Map<PedidoDTO>(pedido);
            dto.Estado = pedido.Estado;
            return Ok(dto);
        }

        // ✅ GET: api/Pedido
        [HttpGet]
        public async Task<ActionResult<List<PedidoDTO>>> GetAll()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Proveedor)
                .ToListAsync();

            var dtos = pedidos.Select(p => new PedidoDTO
            {
                Id = p.Id,
                NroPedido = p.NroPedido,
                Estado = p.Estado,
                IdProveedor = p.IdProveedor,
                Nombre = p.Proveedor?.Nombre
            }).ToList();

            return Ok(dtos);
        }


        // ✅ POST: api/Pedido
        //[HttpPost]
        //public async Task<ActionResult<PedidoDTO>> Post(PedidoDTO dto)
        //{
        //    var pedido = _mapper.Map<Pedido>(dto);
        //    _context.Pedidos.Add(pedido);
        //    await _context.SaveChangesAsync();

        //    var resultDto = _mapper.Map<PedidoDTO>(pedido);
        //    return CreatedAtAction(nameof(GetAll), new { id = pedido.Id }, resultDto);
        //}

        [HttpPost]
        public async Task<ActionResult<PedidoDTO>> Post(PedidoDTO dto)
        {
            // 🔍 Validar que el proveedor exista
            var proveedor = await _context.Proveedores.FindAsync(dto.IdProveedor);
            if (proveedor == null)
                return BadRequest($"El proveedor con ID {dto.IdProveedor} no existe.");

            // 🔁 Mapear el DTO a entidad
            var pedido = _mapper.Map<Pedido>(dto);

            // 🎲 Generar NroPedido aleatorio y único
            var random = new Random();
            int nroGenerado;
            do
            {
                nroGenerado = random.Next(100000, 999999);
            }
            while (await _context.Pedidos.AnyAsync(p => p.NroPedido == nroGenerado));
            pedido.NroPedido = nroGenerado;

            // 🔗 Asignar proveedor manualmente
            pedido.Proveedor = proveedor;
            pedido.IdProveedor = proveedor.Id;

            // 💾 Guardar en base de datos
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // 🔁 Mapear entidad a DTO de respuesta
            var resultDto = new PedidoDTO
            {
                Id = pedido.Id,
                NroPedido = pedido.NroPedido,
                Estado = pedido.Estado,
                IdProveedor = pedido.IdProveedor,
                Nombre = proveedor.Nombre
            };

            return CreatedAtAction(nameof(GetAll), new { id = pedido.Id }, resultDto);
        }


    }
}
