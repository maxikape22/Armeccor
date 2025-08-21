using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Areas")]
    public class AreasController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;
        public AreasController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.context = dbContext;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaListaDTO>>> GetAreas()
        {
            var areas = await context.Areas.ToListAsync();
            return Ok(areas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AreaListaDTO>> GetAreaPorId(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == id);

            if (area is null)
            {
                return NotFound();
            }
            return _mapper.Map<AreaListaDTO>(area);
        }

        [HttpGet("NombreArea")]
        public async Task<ActionResult<List<CrearAreaDTO>>> GetAreasPorNombre([FromQuery] string nombreArea)
        {
            var areas = await context.Areas
                .Where(x => x.NombreArea.Contains(nombreArea))
                .ToListAsync();

            if (areas == null || !areas.Any())
                return NotFound($"No existe el área: {nombreArea}");

            return _mapper.Map<List<CrearAreaDTO>>(areas);
        }


        [HttpPost]
        public async Task<ActionResult<CrearAreaDTO>> PostArea(CrearAreaDTO crearAreaDTO)
        {
            var area = _mapper.Map<Area>(crearAreaDTO);
            context.Areas.Add(area);
            await context.SaveChangesAsync();
            var areaDTO = _mapper.Map<CrearAreaDTO>(area);
            return Ok(areaDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var area = await context.Areas.FindAsync(id);
            if (area == null)
            {
                return NotFound($"No se encontró el área de Id: {id}");
            }
            context.Areas.Remove(area);
            await context.SaveChangesAsync();
            var areaDTO = _mapper.Map<CrearAreaDTO>(area);
            return Ok(areaDTO);
        }

        [HttpDelete("BorrarPorNombreArea/{nombreArea}")]
        public async Task<IActionResult> DeleteAreaPorNombre(string nombreArea)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.NombreArea == nombreArea);
            if (area == null)
            {
                return NotFound($"No se encontró el área: {nombreArea}");
            }
            context.Areas.Remove(area);
            await context.SaveChangesAsync();
            var areaDTO = _mapper.Map<CrearAreaDTO>(area);
            return Ok(areaDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutArea(int id, CrearAreaDTO dto)
        {
            var area = await context.Areas.FirstOrDefaultAsync(a => a.Id == id);
            if (area == null) return NotFound();
            _mapper.Map(dto, area);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
