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
    [Route("api/Proveedores")]
    public class ProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProveedorController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this._context = dbContext;
            this._mapper = mapper;
        }

        // ✅ GET: api/Proveedor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDTO>>> GetAll()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            var dtoList = _mapper.Map<List<ProveedorDTO>>(proveedores);
            return Ok(dtoList);
        }

        // ✅ POST: api/Proveedor
        [HttpPost]
        public async Task<ActionResult<ProveedorDTO>> Post(ProveedorDTO dto)
        {
            var proveedor = _mapper.Map<Proveedor>(dto);
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            var resultDto = _mapper.Map<ProveedorDTO>(proveedor);
            return CreatedAtAction(nameof(GetAll), new { id = proveedor.Id }, resultDto);
        }
    }
}
