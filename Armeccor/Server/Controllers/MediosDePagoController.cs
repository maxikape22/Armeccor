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
    [Route("api/MediosDePago")]
    public class MediosDePagoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public MediosDePagoController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<MedioDePagoDTO>>> Get()
        {
            var lista = await _context.MedioDePagos.ToListAsync();
            var medio = _mapper.Map<List<MedioDePagoDTO>>(lista);
            return Ok(medio);
        }

        [HttpPost]
        public async Task<ActionResult<MedioDePago>> Post(MedioDePago medio)
        {
            if (string.IsNullOrWhiteSpace(medio.Nombre_Medio))
                return BadRequest("Nombre de medio vacío");

            // evitar duplicados por nombre (opcional)
            var existe = await _context.MedioDePagos.AnyAsync(m => m.Nombre_Medio == medio.Nombre_Medio);
            if (existe)
            {
                var existente = await _context.MedioDePagos.FirstOrDefaultAsync(m => m.Nombre_Medio == medio.Nombre_Medio);
                return Ok(existente);
            }

            _context.MedioDePagos.Add(medio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = medio.Id }, medio);
        }
    }
}
