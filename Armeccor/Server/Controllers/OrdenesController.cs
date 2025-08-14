using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Ordenes")]
    public class OrdenesController : ControllerBase
    {
        #region
        //private readonly ApplicationDbContext context;
        //private readonly IMapper _mapper;

        //public OrdenesController(ApplicationDbContext context, IMapper mapper)
        //{
        //    this.context = context;
        //    this._mapper = mapper;
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> PutCliente(int id, CrearOrdenDTO ordenActualizacionDto)
        //{
        //    var ordenExistente = await context.Ordenes.FindAsync(id);

        //    if (ordenExistente == null)
        //    {
        //        return NotFound();
        //    }

        //    _mapper.Map(ordenActualizacionDto, ordenExistente);
        //    context.Entry(ordenExistente).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!await context.Ordenes.AnyAsync(e => e.Id == id))
        //        {
        //            return NotFound($"No se pudó actualizar el registro de Id: {id}");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return NoContent();
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CrearOrdenDTO>>> GetClientes()
        //{
        //    var ordenes = await context.Ordenes.ToListAsync();
        //    return Ok(ordenes);
        //}

        //[HttpGet("{id:int}")] 
        //public async Task<ActionResult<CrearOrdenDTO>> GetCliente(int id)
        //{
        //    var orden = await context.Ordenes.FirstOrDefaultAsync(x => x.Id == id);

        //    if (orden is null)
        //    {
        //        return NotFound($"No se encontro el registro solictado de Id:{id}");
        //    }
        //    return _mapper.Map<CrearOrdenDTO>(orden);
        //}

        //[HttpPost]
        //public async Task<ActionResult<CrearClienteDTO>> PostCliente(CrearOrdenDTO crearOrdenDTO)
        //{
        //    var orden = _mapper.Map<Orden>(crearOrdenDTO);

        //    context.Ordenes.Add(orden);

        //    await context.SaveChangesAsync();

        //    var ordenDTO = _mapper.Map<CrearOrdenDTO>(orden);
        //    return Ok(ordenDTO);
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult> DeleteOrden(int id)
        //{
        //    var orden = await context.Ordenes.FindAsync(id);
        //    if (orden == null)
        //    {
        //        return NotFound($"No se pudó borrar el registro de Id:{id}");
        //    }
        //    context.Ordenes.Remove(orden);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}
        #endregion
        
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public OrdenesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        // --- Método PUT para actualizar una Orden por ID ---
        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutOrden(int id, CrearOrdenDTO ordenActualizacionDto)
        {
            var ordenExistente = await context.Ordenes.FindAsync(id);

            if (ordenExistente == null)
            {
                return NotFound($"No se pudo encontrar la orden con ID: {id}");
            }

            _mapper.Map(ordenActualizacionDto, ordenExistente);
            context.Entry(ordenExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Ordenes.AnyAsync(e => e.Id == id))
                {
                    return NotFound($"No se pudo actualizar el registro de Id: {id}. Posiblemente fue borrado.");
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // --- Método GET para obtener TODAS las órdenes (lista con detalles relacionados) ---
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes() // Tipo de retorno cambiado a OrdenDetalleDTO
        {
            var ordenes = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.AreaOrdenes).ThenInclude(ao => ao.Area)
                .Include(o => o.Piezas)
                .Include(o => o.Entregas)
                .Include(o => o.InsumosOrden).ThenInclude(io => io.Insumo)
                .ToListAsync();

            // Mapea la lista de entidades a una lista de OrdenDetalleDTO
            return Ok(_mapper.Map<IEnumerable<OrdenDetalleDTO>>(ordenes));
        }

        // --- Método GET para obtener una Orden por ID con sus detalles relacionados (con DTO) ---
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrdenDetalleDTO>> GetOrden(int id)
        {
            var orden = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.AreaOrdenes).ThenInclude(ao => ao.Area)
                .Include(o => o.Piezas)
                .Include(o => o.Entregas)
                .Include(o => o.InsumosOrden).ThenInclude(io => io.Insumo)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orden is null)
            {
                return NotFound($"No se encontró el registro solicitado de Id: {id}");
            }
            return _mapper.Map<OrdenDetalleDTO>(orden);
        }

        // --- Método POST para crear una Orden ---
        [HttpPost]
        public async Task<ActionResult<CrearOrdenDTO>> PostOrden(CrearOrdenDTO crearOrdenDTO)
        {
            var clienteExiste = await context.Clientes.AnyAsync(c => c.Id == crearOrdenDTO.IdCliente);
            if (!clienteExiste)
            {
                return BadRequest($"El Cliente con ID {crearOrdenDTO.IdCliente} no existe.");
            }


            var orden = _mapper.Map<Orden>(crearOrdenDTO);

            context.Ordenes.Add(orden);

            await context.SaveChangesAsync();

            var ordenDTO = _mapper.Map<CrearOrdenDTO>(orden);
            return Ok(ordenDTO);
        }

        // --- Método DELETE para eliminar una Orden por ID ---
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
    }
}