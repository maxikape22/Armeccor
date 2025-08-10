using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using Armeccor.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Cliente>> Get()
        {
            return await context.Clientes.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

            if (cliente is null)
            {
                return NotFound();
            }

            return cliente;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            context.Add(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id:int}")]
        public ActionResult PutById(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("El Id no coincide con el cliente a actualizar.");
            }

            var clienteExistente = context.Clientes.Where(c => c.Id == id).FirstOrDefault();

            if (clienteExistente == null)
            {
                return NotFound("No se encontró el cliente.");
            }

            clienteExistente.Nombre = cliente.Nombre;

            try
            {
                context.Clientes.Update(clienteExistente);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"No se pudo actualizar el cliente: {e.Message}");
            }
        }


        [HttpPut("dni/{dni}")]
        public ActionResult PutByDni(int dni, [FromBody] Cliente cliente)
        {
            var clienteExistente = context.Clientes.Where(c => c.DNI == dni).FirstOrDefault();

            if (clienteExistente == null)
            {
                return NotFound("No se encontró el cliente con el DNI especificado.");
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.DNI = cliente.DNI;
            clienteExistente.Dirección = cliente.Dirección;
            clienteExistente.Telefono = cliente.Telefono;

            try
            {
                context.Clientes.Update(clienteExistente);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"No se pudo actualizar el cliente: {e.Message}");
            }
        }
    }
}
