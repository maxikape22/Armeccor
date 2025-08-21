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
    [Route("api/Insumos")]
    public class InsumosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public InsumosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CrearInsumoDTO>>> GetInsumos()
        {
            var insumos = await context.Insumos.ToListAsync();
            return Ok(insumos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CrearInsumoDTO>> GetInsumoPorId(int id)
        {
            var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == id);

            if (insumo is null)
            {
                return NotFound($"No se encontró el insumo de Id: {id}");
            }
            return mapper.Map<CrearInsumoDTO>(insumo);
        }


        [HttpPost]
        public async Task<ActionResult<CrearInsumoDTO>> PostArea(CrearInsumoDTO crearInsumoDTO)
        {
            var insumo = mapper.Map<Insumo>(crearInsumoDTO);
            context.Insumos.Add(insumo);
            await context.SaveChangesAsync();
            var insumoDTO = mapper.Map<CrearInsumoDTO>(insumo);
            return Ok(insumoDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            var insumo = await context.Insumos.FindAsync(id);
            if (insumo == null)
            {
                return NotFound($"No se encontró el insumo de Id: {id}");
            }
            context.Insumos.Remove(insumo);
            await context.SaveChangesAsync();
            var areaDTO = mapper.Map<CrearInsumoDTO>(insumo);
            return Ok(areaDTO);
        }

        [HttpDelete("NombreInsumo/{nombreInsumo}")]
        public async Task<IActionResult> DeleteInsumoPorNombre(string nombreInsumo)
        {
            var insumo = await context.Insumos.FindAsync(nombreInsumo);
            if (insumo == null)
            {
                return NotFound($"No se pudó borrar el insumo de nombre: {nombreInsumo}");
            }
            context.Insumos.Remove(insumo);
            await context.SaveChangesAsync();
            var areaDTO = mapper.Map<CrearInsumoDTO>(insumo);
            return Ok(areaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutInsumo(int id, CrearInsumoDTO dto)
        {
            var insumo = await context.Insumos.FirstOrDefaultAsync(a => a.Id == id);
            if (insumo == null) return NotFound();
            mapper.Map(dto, insumo);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
