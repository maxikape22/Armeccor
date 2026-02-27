using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using Armeccor.Datos.Migrations;
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
    [Route("api/Clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        public ClientesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCliente(int id, CrearClienteDTO dto)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(a => a.Id == id);
            if (cliente == null) return NotFound();
            _mapper.Map(dto, cliente);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrearClienteDTO>>> GetClientes()
        {
            var clientes = await context.Clientes.Where(e=>e.EstaActivo).ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CrearClienteDTO>> GetCliente(int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);
            if (cliente is null)
            {
                return NotFound();
            }
            return _mapper.Map<CrearClienteDTO>(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<CrearClienteDTO>> PostCliente(CrearClienteDTO crearClienteDTO)
        {
            // 🔎 Verificar si ya existe un cliente con ese DNI
            var existeCliente = await context.Clientes
                .AnyAsync(c => c.DNI == crearClienteDTO.DNI && c.EstaActivo);

            if (existeCliente)
            {
                return BadRequest($"No se puede registrar el cliente. Ya existe un cliente con DNI: {crearClienteDTO.DNI}");
            }

            var cliente = _mapper.Map<Cliente>(crearClienteDTO);

            cliente.EstaActivo = true;
            cliente.FechaBaja = null;

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var clienteDTO = _mapper.Map<CrearClienteDTO>(cliente);

            return Ok(clienteDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(e => e.Id == id);

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            if (!cliente.EstaActivo)
                return BadRequest("El cliente fue dado de baja.");

            cliente.EstaActivo = false;
            cliente.FechaBaja = DateTime.Now;

            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
