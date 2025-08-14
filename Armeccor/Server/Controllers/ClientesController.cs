using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        #region
        //[HttpGet]
        //public async Task<IEnumerable<Cliente>> Get()
        //{
        //    return await context.Clientes.ToListAsync();
        //}
        #endregion

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCliente(int id, CrearClienteDTO clienteActualizacionDto)
        {
            var clienteExistente = await context.Clientes.FindAsync(id);

            if (clienteExistente == null)
            {
                return NotFound();
            }

            _mapper.Map(clienteActualizacionDto, clienteExistente);
            context.Entry(clienteExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Clientes.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpGet] 
        public async Task<ActionResult<IEnumerable<CrearClienteDTO>>> GetClientes() 
        {            var clientes = await context.Clientes.ToListAsync();

            //return _mapper.Map<IEnumerable<CrearClienteDTO>>(clientes);
            return Ok(clientes);
        }

        [HttpGet("{id:int}")] // Atributo HttpGet con restricción de tipo entero
        public async Task<ActionResult<CrearClienteDTO>> GetCliente(int id) 
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id); 

            if (cliente is null)
            {
                return NotFound();
            }
            // Mapear la entidad Cliente a ClienteDTO antes de devolverla
            return _mapper.Map<CrearClienteDTO>(cliente);
        }

        #region
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Cliente>> Get(int id)
        //{
        //    var cliente = await context.Clientes.FirstOrDefaultAsync(x => x.Id == id);

        //    if (cliente is null)
        //    {
        //        return NotFound();
        //    }

        //    return cliente;
        //}

        //[HttpPost]
        //public async Task<ActionResult> Post(Cliente cliente)
        //{
        //    context.Add(cliente);
        //    await context.SaveChangesAsync();
        //    return Ok();
        //}

        #endregion
        [HttpPost]
        public async Task<ActionResult<CrearClienteDTO>> PostCliente(CrearClienteDTO crearClienteDTO)
        {
            var cliente = _mapper.Map<Cliente>(crearClienteDTO);

            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();

            var clienteDTO = _mapper.Map<CrearClienteDTO>(cliente);
            return Ok(clienteDTO);
        }
        #region
        //[HttpPut("{id:int}")]
        //public ActionResult PutById(int id, [FromBody] Cliente cliente)
        //{
        //    if (id != cliente.Id)
        //    {
        //        return BadRequest("El Id no coincide con el cliente a actualizar.");
        //    }

        //    var clienteExistente = context.Clientes.Where(c => c.Id == id).FirstOrDefault();

        //    if (clienteExistente == null)
        //    {
        //        return NotFound("No se encontró el cliente.");
        //    }

        //    clienteExistente.Nombre = cliente.Nombre;
        //    clienteExistente.DNI = cliente.DNI;
        //    clienteExistente.Telefono = cliente.Telefono;
        //    clienteExistente.Dirección = cliente.Dirección;

        //    try
        //    {
        //        context.Clientes.Update(clienteExistente);
        //        context.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest($"No se pudo actualizar el cliente: {e.Message}");
        //    }
        //}
        #endregion
    }
}
