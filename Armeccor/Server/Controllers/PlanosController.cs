using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Controllers
{
    [ApiController]
    [Route("api/Planos")]
    public class PlanoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PlanoController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //[HttpPost("SubirPlano")]
        //[Consumes("multipart/form-data")]
        //public async Task<ActionResult> SubirPlano(IFormFile archivo, int OrdenId)
        //{

        //    {
        //        if (archivo == null || archivo.Length == 0)
        //            return BadRequest("No se ha proporcionado ningún archivo.");

        //        var orden = await context.Ordenes
        //            .FirstOrDefaultAsync(o => o.Id == OrdenId); // ✅ usar el ID recibido

        //        if (orden == null)
        //            return NotFound($"No se encontró la orden con ID {OrdenId}.");

        //        var nombreCarpeta = orden.NombreOrden.Replace(" ", "_");
        //        var carpetaDestino = Path.Combine("C:\\Planos", nombreCarpeta);
        //        if (!Directory.Exists(carpetaDestino))
        //            Directory.CreateDirectory(carpetaDestino);

        //        var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
        //        var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

        //        using var stream = new FileStream(rutaCompleta, FileMode.Create);
        //        await archivo.CopyToAsync(stream);

        //        var plano = new Plano
        //        {
        //            RutaSVG = rutaCompleta,
        //            RutaOriginal = archivo.FileName,
        //            FechaCreacion = DateTime.UtcNow,
        //            OrdenId = orden.Id
        //        };

        //        context.Planos.Add(plano);
        //        await context.SaveChangesAsync();

        //        return Ok(new
        //        {
        //            mensaje = "Plano subido y asociado correctamente.",
        //            plano.Id,
        //            plano.RutaSVG,
        //            plano.RutaOriginal,
        //            plano.FechaCreacion,
        //            plano.OrdenId
        //        });

        //    }

        //}

        [HttpGet("PorOrdenActual")]
        public async Task<ActionResult<List<Plano>>> ObtenerPlanosDeOrdenActual()
        {
            // 🔍 Lógica para determinar la orden actual
            var ordenActual = await context.Ordenes
                .OrderByDescending(o => o.FechaInicio)
                .FirstOrDefaultAsync();

            if (ordenActual == null)
                return NotFound("No hay ninguna orden activa.");

            // 🔗 Obtener todos los planos asociados
            var planos = await context.Planos
                .Where(p => p.OrdenId == ordenActual.Id)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

            return Ok(planos);
        }

        [HttpGet("PorNroOT/{nroOT}")]
        public async Task<ActionResult<List<PlanoFiltroDTO>>> ObtenerPlanosPorNroOT(int nroOT)
        {
            var orden = await context.Ordenes
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.NroOT == nroOT);

            if (orden == null)
                return NotFound($"No se encontró ninguna orden con NroOT: {nroOT}");

            var planos = await context.Planos
                .Where(p => p.OrdenId == orden.Id)
                .OrderByDescending(p => p.FechaCreacion)
                .Select(p => new PlanoFiltroDTO
                {
                    Id = p.Id,
                    RutaSVG = p.RutaSVG,
                    RutaOriginal = p.RutaOriginal,
                    FechaCreacion = p.FechaCreacion,
                    NombreOrden = orden.NombreOrden,
                    OrdenId = orden.Id
                })
                .ToListAsync();

            return Ok(planos);
        }


        ////////////////
        ///


        [HttpPost("SubirPlano")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PlanoFiltroDTO>> SubirPlano(IFormFile archivo, int NroOT)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("No se ha proporcionado ningún archivo.");

            var orden = await context.Ordenes
                .FirstOrDefaultAsync(o => o.NroOT == NroOT);

            if (orden == null)
                return NotFound($"No se encontró la orden N°: {NroOT} para asociar al plano.");

            var nombreCarpeta = orden.NombreOrden.Replace(" ", "_");
            var carpetaDestino = Path.Combine("C:\\Planos", nombreCarpeta);
            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
            var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            using var stream = new FileStream(rutaCompleta, FileMode.Create);
            await archivo.CopyToAsync(stream);

            var plano = new Plano
            {
                RutaSVG = rutaCompleta,
                RutaOriginal = archivo.FileName,
                FechaCreacion = DateTime.UtcNow,
                OrdenId = orden.Id,

            };

            context.Planos.Add(plano);
            await context.SaveChangesAsync();

            var planoDTO = mapper.Map<PlanoFiltroDTO>(plano);
            planoDTO.NroOT = orden.NroOT;
            planoDTO.NombreOrden = orden.NombreOrden;

            return Ok(planoDTO);
        }

        //[HttpPost("SubirPlano")]
        //[Consumes("multipart/form-data")]
        //public async Task<ActionResult<PlanoFiltroDTO>> SubirPlano(IFormFile archivo, int NroOT)
        //{
        //    if (archivo == null || archivo.Length == 0)
        //        return BadRequest("No se ha proporcionado ningún archivo.");

        //    var orden = await context.Ordenes
        //        .FirstOrDefaultAsync(o => o.NroOT == NroOT);

        //    if (orden == null)
        //        return NotFound($"No se encontró la orden N°: {NroOT} para asociar al plano.");

        //    var nombreCarpeta = orden.NombreOrden.Replace(" ", "_");
        //    var carpetaDestino = Path.Combine("C:\\Planos", nombreCarpeta);
        //    if (!Directory.Exists(carpetaDestino))
        //        Directory.CreateDirectory(carpetaDestino);

        //    var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
        //    var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

        //    using var stream = new FileStream(rutaCompleta, FileMode.Create);
        //    await archivo.CopyToAsync(stream);

        //    // ✅ Generar ruta pública para Blazor
        //    var rutaRelativa = Path.Combine(nombreCarpeta, nombreArchivo).Replace("\\", "/");
        //    var rutaPublica = $"http://localhost:7253/archivos/{rutaRelativa}";

        //    var plano = new Plano
        //    {
        //        RutaSVG = rutaPublica,
        //        RutaOriginal = archivo.FileName,
        //        FechaCreacion = DateTime.UtcNow,
        //        OrdenId = orden.Id
        //    };

        //    context.Planos.Add(plano);
        //    await context.SaveChangesAsync();

        //    var planoDTO = mapper.Map<PlanoFiltroDTO>(plano);
        //    planoDTO.NroOT = orden.NroOT;
        //    planoDTO.NombreOrden = orden.NombreOrden;

        //    return Ok(planoDTO);
        //}


        [HttpPost("AbrirRutaLocal")]
        public IActionResult AbrirRutaLocal([FromBody] string ruta)
        {
            if (!System.IO.File.Exists(ruta) && !Directory.Exists(ruta))
                return NotFound("Ruta no válida.");

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = ruta,
                    UseShellExecute = true
                });

                return Ok("Ruta abierta.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al abrir la ruta: {ex.Message}");
            }
        }



    }
}