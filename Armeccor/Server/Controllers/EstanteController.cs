using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/Estantes")]
public class EstantesController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    public EstantesController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    //[HttpGet]
    //public async Task<ActionResult<List<Estante>>> Get()
    //{
    //    return await context.Estantes
    //        .Where(e => e.Activo)
    //        .OrderBy(e => e.Codigo)
    //        .ToListAsync();
    //}

    [HttpGet]
    public async Task<ActionResult<List<EstanteDTO>>> GetEstantes()
    {
        var estantes = await context.Estantes
            .Where(e => e.Activo)
            .Select(e => new EstanteDTO
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Descripcion = e.Descripcion,
                Activo = e.Activo,
                NombreInsumo = e.Insumos
                    .OrderBy(i => i.Nombre)
                    .Select(i => i.Nombre)
                    .FirstOrDefault(),
                CantidadInsumo = e.Insumos
                    .Sum(i => (decimal?)i.CantDisponible) ?? 0,
            })
            .ToListAsync();

        return Ok(estantes);
    }


    //[HttpPost]
    //public async Task<ActionResult> Post(Estante estante)
    //{
    //    context.Estantes.Add(estante);
    //    await context.SaveChangesAsync();
    //    return Ok(estante);
    //}

    [HttpPost]
    public async Task<ActionResult> PostEstante(EstanteDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            return BadRequest("La descripción es obligatoria.");

        var codigo = $"E-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

        var estante = mapper.Map<Estante>(dto);

        estante.Codigo = codigo;
        estante.Activo = true;              // ✅ ACÁ
        estante.FechaBaja = null;           // ✅ por seguridad

        if (dto.InsumoId.HasValue)
        {
            var insumo = await context.Insumos
                .FirstOrDefaultAsync(i => i.Id == dto.InsumoId.Value);

            if (insumo == null)
                return BadRequest("El insumo seleccionado no existe.");

            insumo.Estante = estante;
        }

        context.Estantes.Add(estante);
        await context.SaveChangesAsync();

        return Ok(new
        {
            estante.Id,
            estante.Codigo,
            estante.Descripcion,
            estante.Activo
        });
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLogicoEstante(int id)
    {
        var estante = await context.Estantes.FirstOrDefaultAsync(e => e.Id == id);

        if (estante == null)
            return NotFound("Estante no encontrado.");

        if (!estante.Activo)
            return BadRequest("El estante ya está dado de baja.");

        estante.Activo = false;
        estante.FechaBaja = DateTime.Now;

        await context.SaveChangesAsync();

        return NoContent();
    }

}
