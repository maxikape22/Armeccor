using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using Armeccor.Datos.Migrations;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //[HttpGet]
        //public async Task<ActionResult<List<CrearInsumoDTO>>> GetInsumos()
        //{
        //    var insumos = await context.Insumos.ToListAsync();
        //    return Ok(insumos);
        //}

        [HttpGet]
        public async Task<ActionResult<List<CrearInsumoDTO>>> GetInsumos()
        {
            var insumos = await context.Insumos
                .Where(d=>d.EstaActivo)
                .Include(i => i.UnidadBase)
                .ToListAsync();

            var dto = mapper.Map<List<CrearInsumoDTO>>(insumos);
            return Ok(dto);
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
        public async Task<ActionResult<InsumoConConversionesDTO>> PostInsumo(CrearInsumoDTO crearInsumoDTO)
        {
            if (crearInsumoDTO.CantDisponible < 0)
                return BadRequest("La cantidad disponible no puede ser negativa.");

            // Cargamos todas las unidades y conversiones una sola vez
            var unidades = await context.UnidadMedidas.ToListAsync();
            var conversionesTabla = await context.UnidadConversiones.ToListAsync();

            var unidad = unidades.FirstOrDefault(u => u.Id == crearInsumoDTO.UnidadMedidaId);

            if (unidad == null)
                return BadRequest("La unidad de medida especificada no existe.");

            var existeInsumo = await context.Insumos
                .AnyAsync(i => i.Nombre == crearInsumoDTO.Nombre);

            if (existeInsumo)
                return BadRequest("Ya existe un insumo con ese nombre.");

            var insumo = mapper.Map<Insumo>(crearInsumoDTO);
            insumo.UnidadBase = unidad;

            context.Insumos.Add(insumo);
            await context.SaveChangesAsync();

            // ✅ Calcular conversiones en memoria con la misma lógica que el GET
            var conversiones = ObtenerConversionesEnMemoria(
                insumo.CantDisponible,
                unidad,
                unidades,
                conversionesTabla
            );

            var dto = new InsumoConConversionesDTO
            {
                Id = insumo.Id,
                Item = $"{insumo.Nombre} {insumo.Detalle}",
                Nombre = insumo.Nombre,
                Detalle = insumo.Detalle,
                CantDisponible = insumo.CantDisponible,
                UnidadMedidaId = insumo.UnidadMedidaId,
                UnidadMedidaNombre = unidad.Nombre,
                UnidadMedidaAbreviatura = unidad.Abreviatura,
                CantidadConvertida = Math.Round(conversiones.FirstOrDefault()?.CantidadConvertida ?? 0, 3),
                Conversiones = conversiones
            };

            return Ok(dto);
        }


        [HttpGet("Unidades")]
        public async Task<ActionResult<List<UnidadMedida>>> GetUnidades()
        {
            var unidades = await context.UnidadMedidas.ToListAsync();
            return Ok(unidades);
        }

        [HttpGet("Tipos")]
        public async Task<ActionResult<List<string>>> GetTipos()
        {
            var tipos = await context.UnidadMedidas
                .Select(u => u.Tipo)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            return Ok(tipos);
        }

        [HttpGet("TipoPorInsumo")]
        public async Task<ActionResult<string>> GetTipoPorInsumo(int insumoId)
        {
            var insumo = await context.Insumos
                .Include(i => i.UnidadBase)
                .FirstOrDefaultAsync(i => i.Id == insumoId && i.FechaBorrado == null);

            if (insumo is null)
                return BadRequest("Insumo inválido");

            return Ok(insumo.UnidadBase.Tipo);
        }


        //[HttpGet("UnidadesDeMedidaSegunTipoAsociadosaUnidadesDeFiltradoPorCadaTipo")]
        //public async Task<ActionResult<List<UnidadMedida>>> GetUnidadesPoTipo(string Tipo)
        //{
        //    List<string> permitidas = new();

        //    switch (Tipo)
        //    {
        //        case "Agrupacion":
        //            permitidas = new() { "Unidad", "Caja", "Paquete", "Bolsa", "Barra", "Perfil", "Planchuela", "Rollo" };
        //            break;
        //        case "Longitud":
        //            permitidas = new() { "Metro", "Centímetro", "Pulgada", "Pie", "Yarda" };
        //            break;
        //        case "Masa":
        //            permitidas = new() { "Kilogramo", "Gramo", "Tonelada", "Libra" };
        //            break;
        //        case "Superficie":
        //            permitidas = new() { "Metro cuadrado", "Centímetro cuadrado", "Milímetro cuadrado", "Pie cuadrado" };
        //            break;
        //        case "Volumen":
        //            permitidas = new() { "Litro", "Mililitro", "Metro cúbico", "Centímetro cúbico" };
        //            break;
        //    }

        //    var unidades = await context.UnidadMedidas
        //        .Where(x => x.Tipo == Tipo && permitidas.Contains(x.Nombre))
        //        .ToListAsync();

        //    return Ok(unidades);
        //}

        [HttpGet("UnidadesDeMedidaSegunTipoAsociadosaUnidadesDeFiltradoPorCadaTipo")]
        public async Task<ActionResult<List<UnidadMedida>>> GetUnidadesPoTipo(string tipo)
        {
            List<string> permitidas = tipo switch
            {
                "Agrupacion" => new() { "Unidad", "Caja", "Paquete", "Bolsa", "Barra", "Perfil", "Planchuela", "Rollo" },
                "Longitud" => new() { "Metro", "Centímetro", "Pulgada", "Pie", "Yarda" },
                "Masa" => new() { "Kilogramo", "Gramo", "Tonelada", "Libra" },
                "Superficie" => new() { "Metro cuadrado", "Centímetro cuadrado", "Milímetro cuadrado", "Pie cuadrado" },
                "Volumen" => new() { "Litro", "Mililitro", "Metro cúbico", "Centímetro cúbico" },
                _ => new()
            };

            var unidades = await context.UnidadMedidas
                .Where(x => x.Tipo == tipo && permitidas.Contains(x.Nombre))
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            return Ok(unidades);
        }



        [HttpGet("UnidadesRaw")]
        public async Task<IActionResult> GetUnidadesRaw()
        {
            var result = new List<object>();

            var conn = context.Database.GetDbConnection();

            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM UnidadMedida";

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new
                {
                    Id = reader["Id"],
                    Nombre = reader["Nombre"],
                    Abreviatura = reader["Abreviatura"],
                    EsBase = reader["EsBase"]
                });
            }

            await conn.CloseAsync();

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInsumo(int id)
        {
            //var insumo = await context.Insumos.FindAsync(id);
            //if (insumo == null)
            //{
            //    return NotFound($"No se encontró el insumo de Id: {id}");
            //}
            //insumo.EstaActivo = false;
            //insumo.FechaBorrado = DateTime.Now;
            //context.Insumos.Remove(insumo);
            //await context.SaveChangesAsync();
            //var areaDTO = mapper.Map<CrearInsumoDTO>(insumo);
            //return Ok(areaDTO);


            var estante = await context.Insumos.FirstOrDefaultAsync(e => e.Id == id);

            if (estante == null)
                return NotFound("Insumo no encontrado.");

            if (!estante.EstaActivo)
                return BadRequest("El insumo ya está dado de baja.");

            estante.EstaActivo = false;
            estante.FechaBorrado = DateTime.Now;

            await context.SaveChangesAsync();

            return NoContent();
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



        private static readonly Dictionary<(string origen, string destino), decimal> Factores =
    new()
    {
        // Longitud
        { ("Metro", "Centímetro"), 100m },
        { ("Metro", "Pulgada"), 39.3701m },
        { ("Metro", "Pie"), 3.28084m },
        { ("Metro", "Yarda"), 1.09361m },

        { ("Centímetro", "Metro"), 0.01m },
        { ("Centímetro", "Pulgada"), 0.393701m },
        { ("Centímetro", "Pie"), 0.0328084m },
        { ("Centímetro", "Yarda"), 0.0109361m },

        { ("Pulgada", "Metro"), 0.0254m },
        { ("Pulgada", "Centímetro"), 2.54m },
        { ("Pulgada", "Pie"), 0.0833333m },
        { ("Pulgada", "Yarda"), 0.0277778m },

        { ("Pie", "Metro"), 0.3048m },
        { ("Pie", "Centímetro"), 30.48m },
        { ("Pie", "Pulgada"), 12m },
        { ("Pie", "Yarda"), 0.333333m },

        { ("Yarda", "Metro"), 0.9144m },
        { ("Yarda", "Centímetro"), 91.44m },
        { ("Yarda", "Pulgada"), 36m },
        { ("Yarda", "Pie"), 3m },

        { ("Metro", "Metro"), 1m },
        { ("Centímetro", "Centímetro"), 1m },
        { ("Pulgada", "Pulgada"), 1m },
        { ("Pie", "Pie"), 1m },
        { ("Yarda", "Yarda"), 1m },


         // Superficie 
        { ("Metro cuadrado", "Centímetro cuadrado"), 10000m },
        { ("Metro cuadrado", "Milímetro cuadrado"), 1_000_000m },
        { ("Metro cuadrado", "Pie cuadrado"), 10.7639m },

        { ("Centímetro cuadrado", "Metro cuadrado"), 0.0001m },
        { ("Centímetro cuadrado", "Milímetro cuadrado"), 100m },
        { ("Centímetro cuadrado", "Pie cuadrado"), 0.00107639m },

        { ("Milímetro cuadrado", "Metro cuadrado"), 0.000001m },
        { ("Milímetro cuadrado", "Centímetro cuadrado"), 0.01m },
        { ("Milímetro cuadrado", "Pie cuadrado"), 0.00000107639m },

        { ("Pie cuadrado", "Metro cuadrado"), 0.092903m },
        { ("Pie cuadrado", "Centímetro cuadrado"), 929.03m },
        { ("Pie cuadrado", "Milímetro cuadrado"), 92903.04m },

        { ("Metro cuadrado", "Metro cuadrado"), 1m },
        { ("Centímetro cuadrado", "Centímetro cuadrado"), 1m },
        { ("Milímetro cuadrado", "Milímetro cuadrado"), 1m },
        { ("Pie cuadrado", "Pie cuadrado"), 1m },

        // Volumen
        { ("Metro cúbico", "Centímetro cúbico"), 1_000_000m },
        { ("Centímetro cúbico", "Metro cúbico"), 0.000001m },
        { ("Metro cúbico", "Litro"), 1000m },
        { ("Litro", "Metro cúbico"), 0.001m },
        { ("Metro cúbico", "Mililitro"), 1_000_000m },
        { ("Mililitro", "Metro cúbico"), 0.000001m },
        { ("Litro", "Mililitro"), 1000m },
        { ("Mililitro", "Litro"), 0.001m },
        { ("Litro", "Centímetro cúbico"), 1000m },
        { ("Centímetro cúbico", "Litro"), 0.001m },
        { ("Mililitro", "Centímetro cúbico"), 1m },
        { ("Centímetro cúbico", "Mililitro"), 1m },
        { ("Litro", "Litro"), 1m },
        { ("Mililitro", "Mililitro"), 1m },
        { ("Metro cúbico", "Metro cúbico"), 1m },
        { ("Centímetro cúbico", "Centímetro cúbico"), 1m },


        // Masa
        { ("Gramo", "Kilogramo"), 0.001m },
        { ("Gramo", "Tonelada"), 0.0000001m },
        { ("Gramo", "Libra"), 0.00220462m },
        { ("Libra", "Gramo"), 453.6m },
        { ("Tonelada", "Gramo"), 1000000m },
        { ("Kilogramo", "Gramo"), 1000m },
        { ("Kilogramo", "Tonelada"), 0.001m },
        { ("Tonelada", "Kilogramo"), 1000m },
        { ("Kilogramo", "Libra"), 2.20462m },
        { ("Libra", "Kilogramo"), 0.453592m },
        { ("Tonelada", "Libra"), 2204.62m },
        { ("Libra", "Tonelada"), 0.000453592m },

        { ("Gramo", "Gramo"), 1m },
        { ("Kilogramo", "Kilogramo"), 1m },
        { ("Tonelada", "Tonelada"), 1m },
        { ("Libra", "Libra"), 1m },};

        

        private decimal? ConvertirEntreUnidadesEnMemoria(
    decimal cantidad,
    UnidadMedida origen,
    UnidadMedida destino,
    List<UnidadConversion> conversiones)
        {
            if (cantidad == 0 || origen == null || destino == null)
                return 0;

            // 1) Buscar conversión directa en BD
            var directa = conversiones.FirstOrDefault(c =>
                c.UnidadOrigenId == origen.Id && c.UnidadDestinoId == destino.Id);
            if (directa != null)
                return cantidad * directa.FactorConversion;

            // 2) Buscar conversión inversa en BD
            var inversa = conversiones.FirstOrDefault(c =>
                c.UnidadOrigenId == destino.Id && c.UnidadDestinoId == origen.Id);
            if (inversa != null && inversa.FactorConversion != 0)
                return cantidad / inversa.FactorConversion;

            // 3) Buscar en diccionario hardcodeado
            if (Factores.TryGetValue((origen.Nombre, destino.Nombre), out var factor))
                return cantidad * factor;

            // 4) No hay conversión definida
            return null;
        }




        private List<ConversionUnidadResultadoDTO> ObtenerConversionesEnMemoria(
    decimal cantidad,
    UnidadMedida origen,
    List<UnidadMedida> unidades,
    List<UnidadConversion> conversiones)
        {
            var resultado = new List<ConversionUnidadResultadoDTO>();

            //if (cantidad == 0)
            //    return resultado;

            if (cantidad == 0)
            {
                resultado.Add(new ConversionUnidadResultadoDTO
                {
                    UnidadOrigenId = origen.Id,
                    UnidadOrigenNombre = origen.Nombre,
                    UnidadOrigenAbreviatura = origen.Abreviatura,
                    UnidadDestinoId = 0,
                    UnidadDestinoNombre = "Sin insumos para convertir",
                    UnidadDestinoAbreviatura = "",
                    CantidadOrigen = 0,
                    CantidadConvertida = 0
                });
                return resultado;
            }

            if (origen.Nombre == "Caja")
            {
                resultado.Add(new ConversionUnidadResultadoDTO
                {
                    UnidadOrigenId = origen.Id,
                    UnidadOrigenNombre = origen.Nombre,
                    UnidadOrigenAbreviatura = origen.Abreviatura,
                    UnidadDestinoId = origen.Id,
                    UnidadDestinoNombre = origen.Nombre,
                    UnidadDestinoAbreviatura = origen.Abreviatura,
                    CantidadOrigen = cantidad,
                    CantidadConvertida = 1
                });

                return resultado;
            }

            // 🟦 UNIDAD → UNIDAD (identidad)
            if (origen.Nombre == "Unidad")
            {
                resultado.Add(new ConversionUnidadResultadoDTO
                {
                    UnidadOrigenId = origen.Id,
                    UnidadOrigenNombre = origen.Nombre,
                    UnidadOrigenAbreviatura = origen.Abreviatura,
                    UnidadDestinoId = origen.Id,
                    UnidadDestinoNombre = origen.Nombre,
                    UnidadDestinoAbreviatura = origen.Abreviatura,
                    CantidadOrigen = cantidad,
                    CantidadConvertida = cantidad
                });

                return resultado;
            }

            // 🟦 AGRUPACIONES BÁSICAS (Paquete, Bolsa)
            if (origen.Nombre == "Paquete" || origen.Nombre == "Bolsa")
            {
                resultado.Add(new ConversionUnidadResultadoDTO
                {
                    UnidadOrigenId = origen.Id,
                    UnidadOrigenNombre = origen.Nombre,
                    UnidadOrigenAbreviatura = origen.Abreviatura,
                    UnidadDestinoId = origen.Id,
                    UnidadDestinoNombre = origen.Nombre,
                    UnidadDestinoAbreviatura = origen.Abreviatura,
                    CantidadOrigen = cantidad,
                    CantidadConvertida = 1
                });

                return resultado;
            }

            // 🟦 AGRUPACIONES ESPECIALES (Barra, Perfil, Planchuela, Rollo)
            if (origen.Nombre == "Barra" || origen.Nombre == "Perfil" ||
                origen.Nombre == "Planchuela" || origen.Nombre == "Rollo")
            {
                resultado.Add(new ConversionUnidadResultadoDTO
                {
                    UnidadOrigenId = origen.Id,
                    UnidadOrigenNombre = origen.Nombre,
                    UnidadOrigenAbreviatura = origen.Abreviatura,
                    UnidadDestinoId = origen.Id,
                    UnidadDestinoNombre = origen.Nombre,
                    //UnidadDestinoAbreviatura = origen.Abreviatura,
                    UnidadDestinoAbreviatura = "m",
                    CantidadOrigen = cantidad,
                    CantidadConvertida = cantidad * (origen.LongitudPorUnidad ?? 1)
                });

                return resultado;
            }

            var compatibles = unidades
                .Where(u => u.Tipo == origen.Tipo && u.Id != origen.Id)
                .ToList();

            foreach (var destino in compatibles)
            {
                var convertido = ConvertirEntreUnidadesEnMemoria(cantidad, origen, destino, conversiones);

                if (convertido != null)
                {
                    resultado.Add(new ConversionUnidadResultadoDTO
                    {
                        UnidadOrigenId = origen.Id,
                        UnidadOrigenNombre = origen.Nombre,
                        UnidadOrigenAbreviatura = origen.Abreviatura,
                        UnidadDestinoId = destino.Id,
                        UnidadDestinoNombre = destino.Nombre,
                        UnidadDestinoAbreviatura = destino.Abreviatura,
                        CantidadOrigen = cantidad,
                        //CantidadConvertida = Math.Round(convertido.Value, 2)
                        CantidadConvertida = FormatearDecimal(convertido.Value)

                    });
                }
            }

            return resultado;
        }

        [HttpGet("ConConversiones")]
        public async Task<ActionResult<List<InsumoConConversionesDTO>>> GetInsumosConConversiones()
        {
            var unidades = await context.UnidadMedidas.ToListAsync();
            var conversiones = await context.UnidadConversiones.ToListAsync();

            var insumos = await context.Insumos
                .Where(d=>d.EstaActivo)
                .Include(i => i.UnidadBase)
                //.Where(i => i.FechaBorrado == null)
                .ToListAsync();

            var resultado = new List<InsumoConConversionesDTO>();

            foreach (var insumo in insumos)
            {
                var unidadOrigen = unidades.First(u => u.Id == insumo.UnidadMedidaId);

                var conv = ObtenerConversionesEnMemoria(
                    insumo.CantDisponible,
                    unidadOrigen,
                    unidades,
                    conversiones
                );

                resultado.Add(new InsumoConConversionesDTO
                {
                    Id = insumo.Id,
                    Item = $"{insumo.Nombre} {insumo.Detalle}",
                    Nombre = insumo.Nombre,
                    Detalle = insumo.Detalle,
                    CantDisponible = insumo.CantDisponible,
                    UnidadMedidaId = insumo.UnidadMedidaId,
                    UnidadMedidaNombre = unidadOrigen.Nombre,
                    UnidadMedidaAbreviatura = unidadOrigen.Abreviatura,
                    CantidadConvertida = Math.Round(conv.FirstOrDefault()?.CantidadConvertida ?? 0, 2),
                    UnidadConvertidaTexto = conv.FirstOrDefault()?.UnidadDestinoNombre == "Sin insumos para convertir"
    ? "Sin insumos para convertir"
    : $"{(conv.FirstOrDefault()?.CantidadConvertida % 1 == 0
        ? conv.FirstOrDefault()?.CantidadConvertida.ToString("0")
        : conv.FirstOrDefault()?.CantidadConvertida.ToString("0.##"))} {conv.FirstOrDefault()?.UnidadDestinoAbreviatura}",
                    Tipo = unidadOrigen.Tipo,
                    Conversiones = conv
                });
            }

            return Ok(resultado);
        }

        private decimal FormatearDecimal(decimal valor)
        {
            if (valor == 0)
                return 0;
            if (valor < 1 && valor > 0)
                return decimal.Parse(valor.ToString("0.################################################"));
            return Math.Round(valor, 2);
        }

        //Viendo la vendola

        [HttpGet("Convertir")]
        public async Task<ActionResult<ConversionUnidadResultadoDTO>> Convertir(
    decimal cantidad, int unidadOrigenId, int unidadDestinoId)
        {
            var unidades = await context.UnidadMedidas.ToListAsync();
            var conversiones = await context.UnidadConversiones.ToListAsync();

            var origen = unidades.FirstOrDefault(u => u.Id == unidadOrigenId);
            var destino = unidades.FirstOrDefault(u => u.Id == unidadDestinoId);

            if (origen == null || destino == null)
                return BadRequest("Unidades inválidas");

            var valor = ConvertirEntreUnidadesEnMemoria(cantidad, origen, destino, conversiones);

            if (valor is null)
                return BadRequest("No se pudo realizar la conversión entre las unidades especificadas.");

            return Ok(new ConversionUnidadResultadoDTO
            {
                UnidadOrigenId = origen.Id,
                UnidadOrigenNombre = origen.Nombre,
                UnidadOrigenAbreviatura = origen.Abreviatura,
                UnidadDestinoId = destino.Id,
                UnidadDestinoNombre = destino.Nombre,
                UnidadDestinoAbreviatura = destino.Abreviatura,
                CantidadOrigen = cantidad,
                CantidadConvertida = FormatearDecimal(valor.Value)
            });
        }


        [HttpPost("ConvertirYActualizar")]
        public async Task<ActionResult<InsumoConConversionesDTO>> ConvertirYActualizar(
    int insumoId,
    string unidadDestinoNombre)
        {
            var unidades = await context.UnidadMedidas.ToListAsync();
            var conversiones = await context.UnidadConversiones.ToListAsync();

            var insumo = await context.Insumos
                .Include(i => i.UnidadBase)
                .FirstOrDefaultAsync(i => i.Id == insumoId && i.FechaBorrado == null);

            if (insumo is null)
                return NotFound("Insumo no encontrado.");

            var origen = unidades.FirstOrDefault(u => u.Id == insumo.UnidadMedidaId);
            var destino = unidades.FirstOrDefault(u => u.Nombre == unidadDestinoNombre);

            if (origen is null || destino is null)
                return BadRequest("Unidades inválidas.");

            var valor = ConvertirEntreUnidadesEnMemoria(insumo.CantDisponible, origen, destino, conversiones);
            if (valor is null)
                return BadRequest("No se pudo realizar la conversión.");

            // ✅ Persistir el cambio
            insumo.UnidadMedidaId = destino.Id;
            insumo.UnidadBase = destino;
            insumo.CantDisponible = FormatearDecimal(valor.Value);
            insumo.UnidadBase.Tipo = destino.Tipo;

            await context.SaveChangesAsync();

            // 🧠 Recalcular conversiones para mostrar
            var conv = ObtenerConversionesEnMemoria(insumo.CantDisponible, destino, unidades, conversiones);

            return Ok(new InsumoConConversionesDTO
            {
                Id = insumo.Id,
                Item = $"{insumo.Nombre} {insumo.Detalle}",
                Nombre = insumo.Nombre,
                Detalle = insumo.Detalle,
                CantDisponible = insumo.CantDisponible,
                UnidadMedidaId = destino.Id,
                UnidadMedidaNombre = destino.Nombre,
                UnidadMedidaAbreviatura = destino.Abreviatura,
                Tipo = destino.Tipo,
                Conversiones = conv,
                UnidadConvertidaTexto = $"{FormatearDecimal(valor.Value)} {destino.Abreviatura}"
            });
        }



        [HttpGet("ConvertirSoloTexto")]
        public async Task<ActionResult<ConversionUnidadResultadoDTO>> ConvertirSoloTexto(
    int insumoId,
    string unidadOrigenNombre,
    string unidadDestinoNombre)
        {
            var insumo = await context.Insumos
                .Include(i => i.UnidadBase)
                .FirstOrDefaultAsync(i => i.Id == insumoId && i.FechaBorrado == null);

            if (insumo is null || insumo.UnidadBase is null)
            {
                return Ok(new ConversionUnidadResultadoDTO
                {
                    UnidadConvertidaTexto = "Insumo inválido"
                });
            }

            var tipo = insumo.UnidadBase.Tipo;
            var cantidad = insumo.CantDisponible;

            var origen = await context.UnidadMedidas
                .FirstOrDefaultAsync(u => u.Nombre == unidadOrigenNombre && u.Tipo == tipo);
            var destino = await context.UnidadMedidas
                .FirstOrDefaultAsync(u => u.Nombre == unidadDestinoNombre && u.Tipo == tipo);

            if (origen is null || destino is null)
            {
                return Ok(new ConversionUnidadResultadoDTO
                {
                    UnidadConvertidaTexto = "Unidades inválidas"
                });
            }

            if (tipo == "Agrupacion")
            {
                return Ok(new ConversionUnidadResultadoDTO
                {
                    UnidadConvertidaTexto = "No se puede convertir entre si de tipo Agrupación"
                });
            }

            // Buscar en tabla de conversiones
            var conversiones = await context.UnidadConversiones
                .Where(c => (c.UnidadOrigenId == origen.Id && c.UnidadDestinoId == destino.Id) ||
                            (c.UnidadOrigenId == destino.Id && c.UnidadDestinoId == origen.Id))
                .ToListAsync();

            decimal? valor = null;

            var directa = conversiones.FirstOrDefault(c => c.UnidadOrigenId == origen.Id && c.UnidadDestinoId == destino.Id);
            if (directa != null)
            {
                valor = cantidad * directa.FactorConversion;
            }
            else
            {
                var inversa = conversiones.FirstOrDefault(c => c.UnidadOrigenId == destino.Id && c.UnidadDestinoId == origen.Id);
                if (inversa != null && inversa.FactorConversion != 0)
                    valor = cantidad / inversa.FactorConversion;
            }

            // Si no hay en BD, usar diccionario tal como está
            if (!valor.HasValue && Factores.TryGetValue((unidadOrigenNombre, unidadDestinoNombre), out var factor))
            {
                valor = cantidad * factor;
            }

            return Ok(new ConversionUnidadResultadoDTO
            {
                UnidadOrigenNombre = unidadOrigenNombre,
                UnidadDestinoNombre = unidadDestinoNombre,
                CantidadOrigen = cantidad,
                CantidadConvertida = valor ?? 0,
                UnidadConvertidaTexto = valor.HasValue
                    ? $"{FormatearDecimal(valor.Value)} {destino.Abreviatura ?? destino.Nombre}"
                    : "No se pudo convertir"
            });
        }
    }
}
