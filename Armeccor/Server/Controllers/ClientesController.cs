using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
            var clientes = await context.Clientes.ToListAsync();
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
            var cliente = _mapper.Map<Cliente>(crearClienteDTO);

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var clienteDTO = _mapper.Map<CrearClienteDTO>(cliente);
            return Ok(clienteDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound($"No se encontró el cliente: {cliente?.Nombre} para eliminar");
            }
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();
            var clienteDTO = _mapper.Map<CrearClienteDTO>(cliente);
            return Ok(clienteDTO);
        }

    }
}
