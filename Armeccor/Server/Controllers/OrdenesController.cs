using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Ordenes")]
    public class OrdenesController : ControllerBase
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public OrdenesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }


        [HttpGet("SoloOrdenIdPorNroOT/{nroOT:int}")]
        public async Task<ActionResult<int>> GetOrdenIdPorNroOT(int nroOT)
        {
            var ordenId = await context.Ordenes
                .Where(o => o.NroOT == nroOT)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

            if (ordenId == 0)
                return NotFound();

            return Ok(ordenId);
        }

        //Para hacer que el cambio de estado asigne la fecha de entrega al momento de cambiar de estado

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutOrden(int id, CrearOrdenDTO ordenActualizacionDto)
        {
            var ordenExistente = await context.Ordenes.FindAsync(id);

            if (ordenExistente == null)
                return NotFound($"No se pudo encontrar la orden con ID: {id}");

            // Mapear los cambios del DTO a la entidad
            _mapper.Map(ordenActualizacionDto, ordenExistente);

            // ✅ Si el nuevo estado es Finalizado y aún no tiene fecha, la asignamos
            if (ordenActualizacionDto.Estado.Equals("Finalizada", StringComparison.OrdinalIgnoreCase) &&
                !ordenExistente.FechaEntrega.HasValue)
            {
                DateTime time = DateTime.Now;
                var contador = time.AddDays(2);
                ordenExistente.FechaEntrega = DateTime.Now; //contador; //DateTime.Now;
            }

            context.Entry(ordenExistente).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync(); // ✅ se guarda todo junto
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await context.Ordenes.AnyAsync(e => e.Id == id))
                    return NotFound($"No se pudo actualizar la orden Nro: {ordenExistente.NroOT}.");
                else
                    throw;
            }

            return NoContent();
        }

        [HttpGet("Paginacion2")]
        public async Task<ActionResult> GetOrdenesPaginadas([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 4)
        {
            var query = context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area);

            var totalRegistros = await query.CountAsync();

            var ordenes = await query
                .OrderBy(o => o.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

            foreach (var dto in ordenesDto)
            {
                var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);

                var areaActual = entidad?.AreaDetalleOrdenes?
                    .FirstOrDefault(ad =>
                        ad.Estado != null &&
                        ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));

                dto.AreaActual = areaActual != null
                    ? $"{areaActual.Area?.NombreArea ?? "(Área sin nombre)"} ({areaActual.Estado})"
                    : "No hay área cargada con estado inicial";
            }

            return Ok(new
            {
                TotalRegistros = totalRegistros,
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina),
                Datos = ordenesDto
            });
        }

        //[HttpGet("FiltroPorNombreDescripcion")]
        //public async Task<ActionResult> GetOrdenesFiltradas(
        //    [FromQuery] string? texto = null,
        //    [FromQuery] int pagina = 1,
        //    [FromQuery] int tamanoPagina = 4)
        //{
        //    var query = context.Ordenes
        //        .Include(o => o.Cliente)
        //        .Include(o => o.Plano)
        //        .Include(o => o.Entregas)
        //        .Include(o => o.AreaDetalleOrdenes)
        //            .ThenInclude(ad => ad.Area)
        //        .AsQueryable();

        //    // 🔍 Filtro por nombre o descripción (case-insensitive)
        //    if (!string.IsNullOrWhiteSpace(texto))
        //    {
        //        texto = texto.ToLower();
        //        query = query.Where(o =>
        //            o.NombreOrden.ToLower().Contains(texto) ||
        //            o.Descripcion.ToLower().Contains(texto));
        //    }

        //    var totalRegistros = await query.CountAsync();

        //    var ordenes = await query
        //        .OrderBy(o => o.Id)
        //        .Skip((pagina - 1) * tamanoPagina)
        //        .Take(tamanoPagina)
        //        .ToListAsync();

        //    var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

        //    foreach (var dto in ordenesDto)
        //    {
        //        var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);

        //        var areaActual = entidad?.AreaDetalleOrdenes?
        //            .FirstOrDefault(ad =>
        //                ad.Estado != null &&
        //                ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));

        //        dto.AreaActual = areaActual != null
        //            ? $"{areaActual.Area?.NombreArea ?? "(Área sin nombre)"} ({areaActual.Estado})"
        //            : "No hay área cargada con estado inicial";
        //    }

        //    return Ok(new
        //    {
        //        TotalRegistros = totalRegistros,
        //        PaginaActual = pagina,
        //        TamanoPagina = tamanoPagina,
        //        TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina),
        //        Datos = ordenesDto
        //    });
        //}




        //[HttpGet()]
        //public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes()
        //{
        //    var ordenes = await context.Ordenes
        //        .Include(o => o.Cliente)
        //        .Include(o => o.Plano)
        //        .Include(o => o.Entregas)
        //        .Include(o => o.AreaDetalleOrdenes)
        //        .ThenInclude(ad => ad.Area)
        //        .ToListAsync();

        //    var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

        //    foreach (var dto in ordenesDto)
        //    {
        //        var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);

        //        // Buscar el área con estado "Iniciado"
        //        var areaActual = entidad?.AreaDetalleOrdenes?
        //            .FirstOrDefault(ad =>
        //                ad.Estado != null &&
        //                ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));

        //        // Si hay un área en estado Iniciado, mostrar Nombre + Estado
        //        if (areaActual != null)
        //        {
        //            var nombre = areaActual.Area?.NombreArea ?? "(Área sin nombre)";
        //            dto.AreaActual = $"{nombre} ({areaActual.Estado})";
        //        }
        //        else
        //        {
        //            dto.AreaActual = "No hay área cargada con estado incial";
        //        }
        //    }

        //    return Ok(ordenesDto);
        //}

        [HttpGet("LISTADO")]
        public async Task<ActionResult<IEnumerable<OrdenDetalleDTO>>> GetOrdenes()
        {
            var ordenes = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                .ThenInclude(ad => ad.Area)
                .ToListAsync();

            var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenes);

            foreach (var dto in ordenesDto)
            {
                var entidad = ordenes.FirstOrDefault(o => o.Id == dto.Id);

                // Buscar el área con estado "Iniciado"
                var areaActual = entidad?.AreaDetalleOrdenes?
                    .FirstOrDefault(ad =>
                        ad.Estado != null &&
                        ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));

                // Si hay un área en estado Iniciado, mostrar Nombre + Estado
                if (areaActual != null)
                {
                    var nombre = areaActual.Area?.NombreArea ?? "(Área sin nombre)";
                    dto.AreaActual = $"{nombre} ({areaActual.Estado})";
                }
                else
                {
                    dto.AreaActual = "No hay área cargada con estado incial";
                }
            }

            return Ok(ordenesDto);
        }

        [HttpGet("Original")]
        public async Task<ActionResult<IEnumerable<Orden>>> GetOrdenesOriginal()
        {
            var ordenes = await context.Ordenes
                .Include(x => x.AreaDetalleOrdenes)
                .ThenInclude(y => y.Area)
                .ToListAsync();
            return Ok(ordenes);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrdenDetalleDTO>> GetOrden(int id)
        {
            var orden = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (orden is null)
            {
                return NotFound($"No se encontró el registro solicitado de Id: {id}");
            }
            return _mapper.Map<OrdenDetalleDTO>(orden);
        }

        [HttpPost]
        public async Task<ActionResult<CrearOrdenDTO>> PostOrden(CrearOrdenDTO crearOrdenDTO)
        {
            var clienteExiste = await context.Clientes.AnyAsync(c => c.Id == crearOrdenDTO.ClienteId);
            if (!clienteExiste)
            {
                return BadRequest($"El Cliente con ID {crearOrdenDTO.ClienteId} no existe.");
            }

            var orden = _mapper.Map<Orden>(crearOrdenDTO);

            NoContentResult noContentResult = new NoContentResult();

            if (crearOrdenDTO.FechaPactada <= crearOrdenDTO.FechaInicio) 
                return Conflict("La fecha pactada debe ser mayor a la fecha de inicio.");

            context.Ordenes.Add(orden);
            await context.SaveChangesAsync();
            var ordenDTO = _mapper.Map<CrearOrdenDTO>(orden);
            return Ok(ordenDTO);
        }

        [HttpPost("Original")]
        public async Task<ActionResult<Orden>> PostOrdenOriginal(Orden orden)
        {
            context.Ordenes.Add(orden);
            await context.SaveChangesAsync();
            return Ok(orden);
        }

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult> DeleteOrden(int id)
        //{
        //    var orden = await context.Ordenes.FindAsync(id);
        //    if (orden == null)
        //    {
        //        return NotFound($"No se pudo borrar el registro de Id: {id}. No encontrado.");
        //    }
        //    context.Ordenes.Remove(orden);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        [HttpDelete("{NroOT:int}")]
        public async Task<ActionResult> DeleteOrden(int NroOT)
        {
            var orden = await context.Ordenes.FirstOrDefaultAsync(e=>e.NroOT==NroOT);
            if (orden == null)
            {
                return NotFound($"No se pudo borrar la orden N°: {NroOT}. No encontrado.");
            }
            context.Ordenes.Remove(orden);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("OrdenConAreas")]
        public async Task<ActionResult<Orden>> GetOrdenConAreas(int id)
        {
            var orden = await context.Ordenes
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area) // 🔹 Para que traiga el NombreArea
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
                return NotFound();

            return Ok(orden);
        }

        [HttpGet("Metodo de mierda")]
        public async Task<ActionResult> GetOrdenesFiltradasMierda(string nombre)
        {
            var ordenes = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area)
                .Where(o => o.NombreOrden.Contains(nombre) || o.Descripcion.Contains(nombre))
                .ToListAsync();
            return Ok(ordenes);
        }

        [HttpGet("FiltroPorNombreDescripcion")]
        public async Task<ActionResult> GetOrdenesFiltradas(
    [FromQuery] string? texto = null,
    [FromQuery] int pagina = 1,
    [FromQuery] int tamanoPagina = 4)
        {
            // Query base con includes
            var baseQuery = context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area)
                .AsQueryable();

            // Si no hay texto, devolver paginación normal sobre TODO
            if (string.IsNullOrWhiteSpace(texto))
            {
                var totalRegistrosAll = await baseQuery.CountAsync();
                var ordenesPage = await baseQuery
                    .OrderBy(o => o.Id)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .ToListAsync();

                var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenesPage);
                // completar AreaActual como ya hacías
                foreach (var dto in ordenesDto)
                {
                    var entidad = ordenesPage.FirstOrDefault(o => o.Id == dto.Id);
                    var areaActual = entidad?.AreaDetalleOrdenes?
                        .FirstOrDefault(ad => ad.Estado != null && ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));
                    dto.AreaActual = areaActual != null
                        ? $"{areaActual.Area?.NombreArea ?? "(Área sin nombre)"} ({areaActual.Estado})"
                        : "No hay área cargada con estado inicial";
                }

                return Ok(new
                {
                    totalRegistros = totalRegistrosAll,
                    paginaActual = pagina,
                    tamanoPagina = tamanoPagina,
                    totalPaginas = (int)Math.Ceiling(totalRegistrosAll / (double)tamanoPagina),
                    datos = ordenesDto
                });
            }

            // ==== HAY TEXTO: buscamos el primer ID que coincida con el texto ====
            var textoLower = texto.ToLower();

            var idsFiltrados = await baseQuery
                .Where(o => o.NombreOrden.ToLower().Contains(textoLower) || o.Descripcion.ToLower().Contains(textoLower))
                .OrderBy(o => o.Id)
                .Select(o => o.Id)
                .ToListAsync();

            if (!idsFiltrados.Any())
            {
                return Ok(new
                {
                    totalRegistros = 0,
                    paginaActual = 1,
                    tamanoPagina = tamanoPagina,
                    totalPaginas = 1,
                    datos = new List<OrdenDetalleDTO>()
                });
            }

            // Primer ID coincidente
            var primerId = idsFiltrados.First();

            // Calcular cuántos registros globales están antes de ese ID (esto evita traer toda la lista)
            var indexGlobal = await context.Ordenes.Where(o => o.Id < primerId).CountAsync();
            var paginaDelRegistro = (indexGlobal / tamanoPagina) + 1;

            // Recuperar la página correspondiente del conjunto GLOBAL (no filtrado) para que el usuario vea la orden en su posición real
            var totalRegistrosGlobal = await context.Ordenes.CountAsync();

            var ordenesPageGlobal = await context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area)
                .OrderBy(o => o.Id)
                .Skip((paginaDelRegistro - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            var ordenesDtoGlobal = _mapper.Map<List<OrdenDetalleDTO>>(ordenesPageGlobal);
            foreach (var dto in ordenesDtoGlobal)
            {
                var entidad = ordenesPageGlobal.FirstOrDefault(o => o.Id == dto.Id);
                var areaActual = entidad?.AreaDetalleOrdenes?
                    .FirstOrDefault(ad => ad.Estado != null && ad.Estado.Equals("Iniciado", StringComparison.OrdinalIgnoreCase));
                dto.AreaActual = areaActual != null
                    ? $"{areaActual.Area?.NombreArea ?? "(Área sin nombre)"} ({areaActual.Estado})"
                    : "No hay área cargada con estado inicial";
            }

            return Ok(new
            {
                totalRegistros = totalRegistrosGlobal,
                paginaActual = paginaDelRegistro,
                tamanoPagina = tamanoPagina,
                totalPaginas = (int)Math.Ceiling(totalRegistrosGlobal / (double)tamanoPagina),
                datos = ordenesDtoGlobal
            });
        }

        [HttpGet("FiltroPorNombreDescripcion2")]
        public async Task<ActionResult> GetOrdenesFiltradas2(
    [FromQuery] string? texto = null,
    [FromQuery] int pagina = 1,
    [FromQuery] int tamanoPagina = 4)
        {
            // Query base para empezar a construir
            var baseQuery = context.Ordenes.Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                    .ThenInclude(ad => ad.Area).AsQueryable();

            // 1. Aplicar el filtro de texto simple
            if (!string.IsNullOrWhiteSpace(texto))
            {
                var textoLower = texto.ToLower();
                // Filtra por NombreOrden O Descripcion (como en "Metodo de mierda")
                baseQuery = baseQuery
                    .Where(o => o.NombreOrden.ToLower().Contains(textoLower) ||
                                o.Descripcion.ToLower().Contains(textoLower) ||
                                (o.NroOT).ToString().ToLower().Contains(textoLower));

            }

            // 2. Contar el total de registros en el conjunto FILTRADO
            var totalRegistrosFiltrados = await baseQuery.CountAsync();

            // 3. Aplicar paginación (Skip/Take) sobre los resultados FILTRADOS
            // (Esta lógica es la estándar y respeta los parámetros `pagina` y `tamanoPagina`)
            var ordenesPage = await baseQuery
                .OrderBy(o => o.Id) // Importante ordenar para resultados de paginación consistentes
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            // 4. Mapeo simple de los resultados (NOTA: Si el DTO requiere includes, deben agregarse)
            // Para la máxima simplicidad pedida, omitimos los includes aquí.
            var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenesPage);

            // 5. Devolver la respuesta con los datos de paginación del conjunto filtrado
            return Ok(new
            {
                totalRegistros = totalRegistrosFiltrados,
                paginaActual = pagina, // La página que pidió el cliente
                tamanoPagina = tamanoPagina,
                totalPaginas = (int)Math.Ceiling(totalRegistrosFiltrados / (double)tamanoPagina),
                datos = ordenesDto
            });
        }

        [HttpGet("FiltroPorEstado")]
        public async Task<ActionResult> GetOrdenesPorEstado(
     [FromQuery] string? estado = null,
     [FromQuery] int pagina = 1,
     [FromQuery] int tamanoPagina = 4)
        {
            var baseQuery = context.Ordenes
                .Include(o => o.Cliente)
                .Include(o => o.Plano)
                .Include(o => o.Entregas)
                .Include(o => o.AreaDetalleOrdenes)
                .ThenInclude(ad => ad.Area)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(estado))
                baseQuery = baseQuery.Where(o => o.Estado == estado);

            var totalRegistrosFiltrados = await baseQuery.CountAsync();

            var ordenesPage = await baseQuery
                .OrderBy(o => o.Id)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            var ordenesDto = _mapper.Map<List<OrdenDetalleDTO>>(ordenesPage);

            return Ok(new
            {
                totalRegistros = totalRegistrosFiltrados,
                paginaActual = pagina,
                tamanoPagina = tamanoPagina,
                totalPaginas = (int)Math.Ceiling(totalRegistrosFiltrados / (double)tamanoPagina),
                datos = ordenesDto
            });
        }
    }
}