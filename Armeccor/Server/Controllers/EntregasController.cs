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
    [Route("api/Entregas")]
    public class EntregasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public EntregasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CrearEntregaDTO>>> GetEntregas()
        {
            var entregas = await context.Entregas.ToListAsync();
            //return _mapper.Map<IEnumerable<CrearAreaDTO>>(areas);
            return Ok(entregas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CrearEntregaDTO>> GetEntrega(int id)
        {
            var entrega = await context.Entregas.FirstOrDefaultAsync(x => x.Id == id);

            if (entrega is null)
            {
                return NotFound($"No se encontraron entregas con el Id {id}");
            }
            return _mapper.Map<CrearEntregaDTO>(entrega);
        }

        [HttpPost]
        public async Task<ActionResult<CrearEntregaDTO>> PostArea(CrearEntregaDTO crearEntregaDTO)
        {
            //var entrega = _mapper.Map<Entrega>(crearEntregaDTO);
            //context.Entregas.Add(entrega);
            //await context.SaveChangesAsync();
            //var entregaDTO = _mapper.Map<CrearEntregaDTO>(entrega);
            //return Ok(entregaDTO);

            var ordenExiste = await context.Ordenes.AnyAsync(c => c.Id == crearEntregaDTO.IdOrden);
            if (!ordenExiste)
            {
                return BadRequest($"La orden con ID {crearEntregaDTO.IdOrden} no existe.");
            }

            var entrega = _mapper.Map<Entrega>(crearEntregaDTO);

            context.Entregas.Add(entrega);

            await context.SaveChangesAsync();

            var entregaDTO = _mapper.Map<CrearEntregaDTO>(entrega);
            return Ok(entregaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var entrega = await context.Entregas.FindAsync(id);
            if (entrega == null)
            {
                return NotFound();
            }
            context.Entregas.Remove(entrega);
            await context.SaveChangesAsync();
            var entregaDTO = _mapper.Map<CrearEntregaDTO>(entrega);
            return Ok(entregaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutArea(int id, CrearEntregaDTO dto)
        {
            var entrega = await context.Entregas.FirstOrDefaultAsync(a => a.Id == id);
            if (entrega == null) return NotFound();
            _mapper.Map(dto, entrega);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
