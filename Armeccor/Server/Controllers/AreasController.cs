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
        public async Task<ActionResult<IEnumerable<CrearAreaDTO>>> GetAreas()
        {
            var areas = await context.Areas.ToListAsync();
            //return _mapper.Map<IEnumerable<CrearAreaDTO>>(areas);
            return Ok(areas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CrearAreaDTO>> GetCliente(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == id);

            if (area is null)
            {
                return NotFound();
            }
            return _mapper.Map<CrearAreaDTO>(area);
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
                return NotFound();
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
