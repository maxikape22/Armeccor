using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using Armeccor.Datos.Migrations;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        private readonly IWebHostEnvironment env;
        public PlanoController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment _web)
        {
            this.context = context;
            this.mapper = mapper;
            this.env = _web;
        }

        [HttpGet("PlanosOrdenadosPorFecha")]
        public async Task<ActionResult<List<PlanoFiltroDTO>>> ObtenerPlanosOrdenadosPorFecha(int NroOT)
        {
            var orden = await context.Ordenes
                .Include(o => o.Planos)
                .FirstOrDefaultAsync(o => o.NroOT == NroOT);

            if (orden == null)
                return NotFound($"No se encontró la orden N°: {NroOT}");

            var planosOrdenados = orden.Planos
                .OrderBy(p => p.FechaCreacion)
                .ToList();

            var planosDTO = mapper.Map<List<PlanoFiltroDTO>>(planosOrdenados);

            foreach (var plano in planosDTO)
            {
                plano.NroOT = orden.NroOT;
                plano.NombreOrden = orden.NombreOrden;
            }

            return Ok(planosDTO);
        }


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
                .Where(o => o.EstaActivo == true)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.NroOT == nroOT);

            if (orden == null)
                return NotFound($"No se encontró ninguna orden con NroOT: {nroOT}");

            var planos = await context.Planos
                .Where(p => p.OrdenId == orden.Id)
                .Where(e=>e.EstaActivo == true)
                .OrderByDescending(p => p.FechaCreacion)
                .Select(p => new PlanoFiltroDTO
                {
                    Id = p.Id,
                    RutaSVG = p.RutaSVG,
                    RutaOriginal = p.RutaOriginal,
                    FechaCreacion = p.FechaCreacion,
                    NombreOrden = orden.NombreOrden,
                    OrdenId = orden.Id,
                    EstaActivo = p.EstaActivo,
                    FechaBaja = p.FechaBaja,
                })
                .ToListAsync();

            return Ok(planos);
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
        //    //var carpetaDestino = Path.Combine("C:\\Planos", nombreCarpeta);
        //    var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Planos", nombreCarpeta);
        //    if (!Directory.Exists(carpetaDestino))
        //        Directory.CreateDirectory(carpetaDestino);

        //    var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
        //    var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

        //    using var stream = new FileStream(rutaCompleta, FileMode.Create);
        //    await archivo.CopyToAsync(stream);

        //    var plano = new Plano
        //    {
        //        RutaSVG = rutaCompleta,
        //        RutaOriginal = archivo.FileName,
        //        FechaCreacion = DateTime.UtcNow,
        //        OrdenId = orden.Id,

        //    };

        //    context.Planos.Add(plano);
        //    await context.SaveChangesAsync();

        //    var planoDTO = mapper.Map<PlanoFiltroDTO>(plano);
        //    planoDTO.NroOT = orden.NroOT;
        //    planoDTO.NombreOrden = orden.NombreOrden;

        //    return Ok(planoDTO);
        //}

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

            // Carpeta dentro de wwwroot/Planos/NombreOrden
            var nombreCarpeta = orden.NombreOrden.Replace(" ", "_");

            var rutaCarpeta = Path.Combine(env.WebRootPath, "Planos", nombreCarpeta);
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            // Nombre final del archivo
            var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
            var rutaFisica = Path.Combine(rutaCarpeta, nombreArchivo);

            // Guardar archivo físicamente
            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            // 👉 Ruta pública que se guarda en la BD (IMPORTANTE)
            var rutaPublica = Path.Combine("Planos", nombreCarpeta, nombreArchivo)
                              .Replace("\\", "/");

            var plano = new Plano
            {
                RutaSVG = rutaPublica,        // ← ESTA es la que vas a mostrar y abrir
                RutaOriginal = archivo.FileName,
                FechaCreacion = DateTime.Now,
                OrdenId = orden.Id
            };

            plano.EstaActivo = true;
            plano.FechaBaja = null;

            context.Planos.Add(plano);
            await context.SaveChangesAsync();

            var planoDTO = mapper.Map<PlanoFiltroDTO>(plano);
            planoDTO.NroOT = orden.NroOT;
            planoDTO.NombreOrden = orden.NombreOrden;

            return Ok(planoDTO);
        }








        //[HttpPost("AbrirRutaLocal")]
        //public IActionResult AbrirRutaLocal([FromBody] string rutaWeb)
        //{
        //    //if (!System.IO.File.Exists(ruta) && !Directory.Exists(ruta))
        //    //    return NotFound("Ruta no válida.");

        //    //try
        //    //{
        //    //    Process.Start(new ProcessStartInfo
        //    //    {
        //    //        FileName = ruta,
        //    //        UseShellExecute = true
        //    //    });

        //    //    return Ok("Ruta abierta.");
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return BadRequest($"Error al abrir la ruta: {ex.Message}");
        //    //}

        //    if (string.IsNullOrWhiteSpace(rutaWeb))
        //        return BadRequest("La ruta recibida es inválida.");

        //    try
        //    {
        //        // Quitar el prefijo inicial si lo trae
        //        if (rutaWeb.StartsWith("/"))
        //            rutaWeb = rutaWeb.Substring(1);

        //        // Convertir la ruta web → ruta física real en wwwroot
        //        var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaWeb.Replace("/", "\\"));

        //        if (!System.IO.File.Exists(rutaFisica))
        //            return NotFound($"No existe el archivo en el servidor: {rutaFisica}");

        //        Process.Start(new ProcessStartInfo
        //        {
        //            FileName = rutaFisica,
        //            UseShellExecute = true
        //        });

        //        return Ok("Plano abierto correctamente en el servidor.");
        //    }
        //    catch (Exception ex)
        //    {
        //        var mensaje= $"Error al intentar abrir el archivo {ex.Message}";
        //        Console.WriteLine(mensaje);
        //        //return BadRequest($"Error al abrir el archivo: {ex.Message}");
        //        return BadRequest(mensaje);
        //    }
        //}

        [HttpPost("AbrirRutaLocal")]
        public IActionResult AbrirRutaLocal([FromBody] string rutaWeb)
        {
            if (string.IsNullOrWhiteSpace(rutaWeb))
                return BadRequest("La ruta recibida es inválida.");

            try
            {
                if (rutaWeb.StartsWith("/"))
                    rutaWeb = rutaWeb.Substring(1);

                var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaWeb.Replace("/", "\\"));

                if (!System.IO.File.Exists(rutaFisica))
                    return NotFound($"No existe el archivo en el servidor: {rutaFisica}");

                // Simplemente devolvemos la URL pública
                var urlPublica = $"{Request.Scheme}://{Request.Host}/{rutaWeb.Replace("\\", "/")}";

                return Ok(urlPublica);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("DescargarZip/{nroOT}")]
        public async Task<IActionResult> DescargarZip(int nroOT)
        {
            var orden = await context.Ordenes
                .FirstOrDefaultAsync(o => o.NroOT == nroOT);

            if (orden == null)
                return NotFound($"No existe la orden N° {nroOT}");

            string nombreCarpeta = orden.NombreOrden.Replace(" ", "_");

            string rutaCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Planos",
                nombreCarpeta
            );

            if (!Directory.Exists(rutaCarpeta))
                return NotFound("Aún no hay planos para esta orden.");

            using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                var archivos = Directory.GetFiles(rutaCarpeta, "*", SearchOption.AllDirectories);

                foreach (var archivo in archivos)
                {
                    // Relativo a la carpeta base
                    string relative = Path.GetRelativePath(rutaCarpeta, archivo);

                    // 🔥 Agregar carpeta raíz dentro del ZIP
                    string entryName = Path.Combine(nombreCarpeta, relative)
                                        .Replace("\\", "/");

                    archive.CreateEntryFromFile(archivo, entryName, CompressionLevel.Fastest);
                }
            }

            memoryStream.Position = 0;

            string nombreZip = $"{nombreCarpeta}_planos.zip";

            return File(memoryStream.ToArray(), "application/zip", nombreZip);
        }

        //[HttpDelete("EliminarPlano")]
        //public async Task<IActionResult> EliminarPlano(int NroOT, string ruta)
        //{
        //    // Buscar la orden con ese NroOT
        //    var orden = await context.Ordenes
        //        .Include(o => o.Planos)
        //        .FirstOrDefaultAsync(o => o.NroOT == NroOT);

        //    if (orden == null)
        //        return NotFound($"No se encontró la orden con NroOT: {NroOT}");

        //    // Buscar el plano dentro de la orden por la ruta
        //    var plano = orden.Planos
        //        .FirstOrDefault(p => p.RutaSVG == ruta);

        //    if (plano == null)
        //        return NotFound($"No se encontró el plano con ruta {ruta} en la orden {NroOT}");

        //    // Eliminar archivo físico
        //    var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ruta.Replace("/", "\\"));
        //    if (System.IO.File.Exists(rutaFisica))
        //    {
        //        System.IO.File.Delete(rutaFisica);
        //    }

        //    if (!plano.EstaActivo)
        //        return BadRequest("El plano ya está dado de baja.");

        //    plano.EstaActivo = false;
        //    plano.FechaBaja = DateTime.Now;

        //    // Eliminar registro en la base de datos
        //    context.Planos.Remove(plano);
        //    await context.SaveChangesAsync();

        //    return Ok($"Plano con ruta {ruta} de la orden {NroOT} eliminado correctamente.");
        //}

        [HttpDelete("EliminarPlano")]
        public async Task<IActionResult> EliminarPlano(int NroOT, string ruta)
        {
            // Buscar la orden con ese NroOT
            var orden = await context.Ordenes
                .Include(o => o.Planos)
                .FirstOrDefaultAsync(o => o.NroOT == NroOT);

            if (orden == null)
                return NotFound($"No se encontró la orden con NroOT: {NroOT}");

            // Buscar el plano dentro de la orden por la ruta
            var plano = orden.Planos
                .FirstOrDefault(p => p.RutaSVG == ruta);

            if (plano == null)
                return NotFound($"No se encontró el plano con ruta {ruta} en la orden {NroOT}");

            if (!plano.EstaActivo)
                return BadRequest("El plano ya está dado de baja.");

            // Baja lógica
            plano.EstaActivo = false;
            plano.FechaBaja = DateTime.Now;

            await context.SaveChangesAsync();

            return Ok($"Plano con ruta {ruta} de la orden {NroOT} dado de baja correctamente.");
        }


        [HttpGet("DescargarPlanoPorRuta")]
        public IActionResult DescargarPlanoPorRuta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return BadRequest("La ruta está vacía.");

            // 🔒 Usar la ruta tal cual, sin modificarla
            var rutaRelativa = ruta.Trim();

            // 🔥 Ruta física: wwwroot + rutaRelativa
            var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rutaRelativa);

            Console.WriteLine($"DescargarPlanoPorRuta -> ruta física: {rutaFisica}");

            if (!System.IO.File.Exists(rutaFisica))
                return NotFound($"No se encontró el archivo físico en: {rutaFisica}");

            var extension = Path.GetExtension(rutaFisica).ToLowerInvariant();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".svg" => "image/svg+xml",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };

            var nombreArchivo = Path.GetFileName(rutaFisica);
            var bytes = System.IO.File.ReadAllBytes(rutaFisica);

            return File(bytes, contentType, nombreArchivo);
        }

    }
}