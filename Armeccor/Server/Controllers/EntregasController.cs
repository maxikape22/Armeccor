using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        #region

        [HttpGet]
        public async Task<IEnumerable<EntregaDetalleDTO>> GetEntregasConOrdenAsync()
        {
            var entregasConOrden = context.Entregas.Include(e => e.Orden).Include(x=>x.Medio_De_Pago);
            var entregasList = await entregasConOrden.ToListAsync();
            var entregasDTO = _mapper.Map<IEnumerable<EntregaDetalleDTO>>(entregasList);
            return entregasDTO;
        }

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<CrearEntregaDTO>> GetEntrega(int id)
        //{
        //    var entrega = await context.Entregas
        //                               .Include(e => e.Orden) // Incluye la relación
        //                               .FirstOrDefaultAsync(x => x.Id == id);

        //    if (entrega is null)
        //    {
        //        return NotFound($"No se encontraron entregas con el Id {id}");
        //    }

        //    return Ok(_mapper.Map<CrearEntregaDTO>(entrega));
        //}


        //[HttpPost]
        //public async Task<ActionResult<CrearEntregaDTO>> PostEntrega(CrearEntregaDTO crearEntregaDTO)
        //{
        //    var ordenExiste = await context.Ordenes.AnyAsync(c => c.Id == crearEntregaDTO.IdOrden);
        //    if (!ordenExiste)
        //    {
        //        return BadRequest($"La orden con ID {crearEntregaDTO.IdOrden} no existe.");
        //    }

        //    var entrega = _mapper.Map<Entrega>(crearEntregaDTO);

        //    context.Entregas.Add(entrega);

        //    await context.SaveChangesAsync();

        //    var entregaDTO = _mapper.Map<CrearEntregaDTO>(entrega);
        //    return Ok(entregaDTO);
        //}
        #endregion


        [HttpPut("ActualizacionPorEstado")]
        public async Task<ActionResult<EntregaDetalleDTO>> ActualizarEstadoEntregaCheckBox(int Id, bool Entregado = true)
        {
            var entrega = await context.Entregas
                .Include(e => e.Orden) // Asumiendo que Entrega tiene navegación a Orden
                .Include(e => e.Medio_De_Pago) // Si existe como entidad relacionada
                .FirstOrDefaultAsync(e => e.Id == Id);

            if (entrega == null) return NotFound();

            entrega.Entregado = Entregado;
            await context.SaveChangesAsync();

            var dto = new EntregaDetalleDTO
            {
                Id = entrega.Id,
                NroOT = entrega.OrdenId,
                NombreOrden = entrega.Orden?.Descripcion ?? "Sin descripción",
                Entregado = entrega.Entregado,
                MedioDePago = entrega.Medio_De_Pago?.Nombre_Medio ?? "No especificado"
            };

            return Ok(dto);
        }




        [HttpPost]
        public async Task<ActionResult<Entrega>> Post(CrearEntregaDTO dto)
        {
            // Validaciones mínimas
            if (dto.IdOrden <= 0)
                return BadRequest("IdOrden inválido.");

            // Si viene NuevoMedio, creamos o recuperamos
            int medioId = dto.MedioDePagoId;
            if (!string.IsNullOrWhiteSpace(dto.NuevoMedio))
            {
                var existing = await context.MedioDePagos
                    .FirstOrDefaultAsync(m => m.Nombre_Medio == dto.NuevoMedio);

                if (existing != null)
                {
                    medioId = existing.Id;
                }
                else
                {
                    var nuevo = new MedioDePago { Nombre_Medio = dto.NuevoMedio };
                    context.MedioDePagos.Add(nuevo);
                    await context.SaveChangesAsync();
                    medioId = nuevo.Id;
                }
            }
            else
            {
                // Si no vino nuevo medio, validar que exista el medio seleccionado
                if (medioId <= 0 || !await context.MedioDePagos.AnyAsync(m => m.Id == medioId))
                    return BadRequest("Medio de pago no válido.");
            }

            // Validar orden
            var orden = await context.Ordenes.FindAsync(dto.IdOrden);
            if (orden == null) return BadRequest("Orden no encontrada.");

            var entidad = new Entrega
            {
                Entregado = dto.Entregado,
                OrdenId = dto.IdOrden,
                MedioDePagoId = medioId
            };

            context.Entregas.Add(entidad);
            await context.SaveChangesAsync();

            // devolver la entidad creada (opcional map a DTO)
            return CreatedAtAction(nameof(GetById), new { id = entidad.Id }, entidad);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Entrega>> GetById(int id)
        {
            var e = await context.Entregas
                .Include(x => x.Medio_De_Pago)
                .Include(x => x.Orden)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEntrega(int id)
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
