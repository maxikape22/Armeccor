using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Armeccor.Server.Controllers
{
    [ApiController]
    [Route("api/Insumo_Detalle_Orden")]
    public class InsumoDetalleOrdenController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public InsumoDetalleOrdenController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this._mapper = mapper;
        }

        // ✅ Obtener todos los detalles
        [HttpGet]
        public async Task<ActionResult<List<InsumoDetalleOrdenListaDTO>>> Get()
        {
            var lista = await context.InsumoDetalleOrdenes.ToListAsync();
            return Ok(lista);
        }

        //// ✅ Agregar o actualizar insumo en orden (suma cantidades)
        //[HttpPost]
        //public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        //{
        //    // Verificar existencia del insumo
        //    var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
        //    if (insumo == null)
        //        return NotFound($"No se encontró el insumo con Id {dto.InsumoId}");

        //    // Verificar existencia de la orden
        //    var orden = await context.Ordenes.FirstOrDefaultAsync(x => x.Id == dto.OrdenId);
        //    if (orden == null)
        //        return NotFound($"No se encontró la orden con Id {dto.OrdenId}");

        //    // Buscar si ya existe un detalle para ese insumo y orden
        //    var detalleExistente = await context.InsumoDetalleOrdenes
        //        .FirstOrDefaultAsync(x => x.OrdenId == dto.OrdenId && x.InsumoId == dto.InsumoId);

        //    if (detalleExistente != null)
        //    {
        //        // 🔹 Si ya existe → sumar cantidad
        //        detalleExistente.Cantidad += dto.Cantidad;
        //        context.InsumoDetalleOrdenes.Update(detalleExistente);
        //    }
        //    else
        //    {
        //        // 🔹 Si no existe → crear nuevo detalle
        //        var nuevoDetalle = new InsumoDetalleOrden
        //        {
        //            OrdenId = dto.OrdenId,
        //            InsumoId = dto.InsumoId,
        //            Cantidad = dto.Cantidad
        //        };
        //        context.InsumoDetalleOrdenes.Add(nuevoDetalle);
        //    }

        //    // 🔹 Actualizar stock (aunque quede negativo)
        //    insumo.CantDisponible -= dto.Cantidad;

        //    await context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        Mensaje = $"Se agregó {dto.Cantidad} de {insumo.Nombre} a la orden {dto.OrdenId}.",
        //        InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo)
        //    });
        //}

        //[HttpPost]
        //public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        //{
        //    var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
        //    if (insumo == null)
        //        return NotFound("Insumo no encontrado");

        //    var orden = await context.Ordenes.FirstOrDefaultAsync(x => x.Id == dto.OrdenId);
        //    if (orden == null)
        //        return NotFound("Orden no encontrada");

        //    var detalle = await context.InsumoDetalleOrdenes
        //        .FirstOrDefaultAsync(x => x.OrdenId == dto.OrdenId && x.InsumoId == dto.InsumoId);

        //    if (detalle != null)
        //        detalle.Cantidad += dto.Cantidad;
        //    else
        //        context.InsumoDetalleOrdenes.Add(new InsumoDetalleOrden
        //        {
        //            OrdenId = dto.OrdenId,
        //            InsumoId = dto.InsumoId,
        //            Cantidad = dto.Cantidad
        //        });

        //    // ✅ DESCUENTO FÍSICO CORRECTO
        //    if (insumo.CantDisponible >= dto.Cantidad)
        //        insumo.CantDisponible -= dto.Cantidad;
        //    else
        //        insumo.CantDisponible = 0;

        //    await context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo)
        //    });
        //}

        [HttpPost]
        public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        {
            var insumo = await context.Insumos
                .FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo == null)
                return NotFound("Insumo no encontrado");

            var orden = await context.Ordenes
                .FirstOrDefaultAsync(x => x.Id == dto.OrdenId);
            if (orden == null)
                return NotFound("Orden no encontrada");

            var detalle = await context.InsumoDetalleOrdenes
                .FirstOrDefaultAsync(x =>
                    x.OrdenId == dto.OrdenId &&
                    x.InsumoId == dto.InsumoId);

            // 🔹 cálculo BASE (NO rompe nada)
            int descontadoAhora = (int)Math.Min(insumo.CantDisponible, dto.Cantidad);
            int pendienteAhora = dto.Cantidad - descontadoAhora;

            if (detalle != null)
            {
                detalle.Cantidad += dto.Cantidad;

                // ✅ NUEVO
                detalle.CantidadDescontada += descontadoAhora;
                detalle.CantidadPendiente += pendienteAhora;
                detalle.Insuficiente = detalle.CantidadPendiente > 0;
            }
            else
            {
                context.InsumoDetalleOrdenes.Add(new InsumoDetalleOrden
                {
                    OrdenId = dto.OrdenId,
                    InsumoId = dto.InsumoId,
                    Cantidad = dto.Cantidad,

                    // ✅ NUEVO
                    CantidadDescontada = descontadoAhora,
                    CantidadPendiente = pendienteAhora,
                    Insuficiente = pendienteAhora > 0
                });
            }

            // ✅ DESCUENTO FÍSICO (LO DEJAMOS IGUAL, SOLO MÁS EXACTO)
            if (insumo.CantDisponible >= dto.Cantidad)
                insumo.CantDisponible -= dto.Cantidad;
            else
                insumo.CantDisponible = 0;

            await context.SaveChangesAsync();

            return Ok(new
            {
                InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo)
            });
        }




        //[HttpPost]
        //public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        //{
        //    if (dto.Cantidad <= 0)
        //        return BadRequest("Cantidad inválida.");

        //    var insumo = await context.Insumos
        //        .FirstOrDefaultAsync(x => x.Id == dto.InsumoId);

        //    if (insumo == null)
        //        return NotFound("Insumo no encontrado.");

        //    var detalle = await context.InsumoDetalleOrdenes
        //        .FirstOrDefaultAsync(x =>
        //            x.OrdenId == dto.OrdenId &&
        //            x.InsumoId == dto.InsumoId);

        //    if (detalle == null)
        //    {
        //        detalle = new InsumoDetalleOrden
        //        {
        //            OrdenId = dto.OrdenId,
        //            InsumoId = dto.InsumoId,
        //            Cantidad = 0,
        //            CantidadDescontada = 0,
        //            CantidadPendiente = 0,
        //            Insuficiente = false
        //        };

        //        context.InsumoDetalleOrdenes.Add(detalle);
        //    }

        //    // 👉 SOLO se acumula lo pedido
        //    detalle.Cantidad += dto.Cantidad;

        //    // 👉 descuento real SOLO si hay stock
        //    int descontar = Math.Min(insumo.CantDisponible, dto.Cantidad);
        //    insumo.CantDisponible -= descontar;

        //    detalle.CantidadDescontada += descontar;

        //    await context.SaveChangesAsync();

        //    // 🔴 lógica pesada va a otro método
        //    await RecalcularEstadoInsumo(detalle.Id);

        //    return Ok();
        //}

        //private async Task RecalcularEstadoInsumo(int detalleId)
        //{
        //    var detalle = await context.InsumoDetalleOrdenes
        //        .FirstAsync(x => x.Id == detalleId);

        //    // 🔹 lo pendiente es TODO lo que no salió del stock
        //    detalle.CantidadPendiente =
        //        detalle.Cantidad - detalle.CantidadDescontada;

        //    // 🔹 si hay pendiente → insuficiente
        //    detalle.Insuficiente = detalle.CantidadPendiente > 0;

        //    await context.SaveChangesAsync();
        //}






        //[HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        //public async Task<ActionResult> GetInsumosAgrupadosPorOrden(int ordenId)
        //{
        //    var query = await context.InsumoDetalleOrdenes
        //        .Include(x => x.Insumo)
        //        .Where(x => x.OrdenId == ordenId)
        //        .GroupBy(x => new { x.InsumoId, x.Insumo.Nombre })
        //        .Select(g => new InsumoDetalleOrdenDTO
        //        {
        //            InsumoId = g.Key.InsumoId.Value,
        //            Nombre = g.Key.Nombre,
        //            Cantidad = g.Sum(x => x.Cantidad), // 👈 suma positiva o negativa
        //            OrdenId = ordenId
        //        })
        //        .ToListAsync();

        //    return Ok(query);
        //}

        //[HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        //public async Task<ActionResult<List<InsumoDetalleOrdenDTO>>>GetInsumosAgrupadosPorOrden(int ordenId)
        //{
        //    var lista = await context.InsumoDetalleOrdenes
        //        .Where(x => x.OrdenId == ordenId)
        //        .Include(x => x.Insumo)

        //        // 🔹 PROYECCIÓN REAL
        //        .ProjectTo<InsumoDetalleOrdenDTO>(_mapper.ConfigurationProvider)
        //        .ToListAsync();

        //    return Ok(lista);
        //}

        [HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        public async Task<ActionResult<List<InsumoDetalleOrdenDTO>>>GetInsumosAgrupadosPorOrden(int ordenId)
        {
            var lista = await context.InsumoDetalleOrdenes
                .Include(x => x.Insumo)
                .Where(x => x.OrdenId == ordenId)
                .Select(x => new InsumoDetalleOrdenDTO
                {
                    InsumoId = x.InsumoId.Value,
                    OrdenId = x.OrdenId.Value,
                    Nombre = x.Insumo.Nombre,
                    NroOT = x.Orden.NroOT,
                    // 🔹 valores REALES persistidos
                    Cantidad = x.Cantidad,
                    CantidadDescontada = x.CantidadDescontada,
                    CantidadPendiente = x.CantidadPendiente,

                    // 🔹 estado DERIVADO (fuente de verdad)
                    Insuficiente = x.CantidadPendiente > 0
                })
                .ToListAsync();

            return Ok(lista);
        }



        //[HttpGet("AgrupadosPorOrden/{ordenId:int}")]
        //public async Task<ActionResult> GetInsumosAgrupadosPorOrden(int ordenId)
        //{
        //    var lista = await context.InsumoDetalleOrdenes
        //        .Include(x => x.Insumo)
        //        .Where(x => x.OrdenId == ordenId)
        //        .Select(x => new InsumoDetalleOrdenDTO
        //        {
        //            Id = x.Id,
        //            InsumoId = x.InsumoId.Value,
        //            OrdenId = x.OrdenId.Value,
        //            Nombre = x.Insumo.Nombre,

        //            Cantidad = x.Cantidad,
        //            CantidadDescontada = x.CantidadDescontada,
        //            CantidadPendiente = x.CantidadPendiente,
        //            Insuficiente = x.Insuficiente
        //        })
        //        .ToListAsync();

        //    return Ok(lista);
        //}




        //qmetodo para liberar los insumos que tienen cargados una orden ,
        [HttpPost("Liberar")]
        public async Task<ActionResult> LiberarInsumo(LiberarInsumoDTO dto)
        {
            // Buscar el detalle de la orden para este insumo
            var detalle = await context.InsumoDetalleOrdenes
                .FirstOrDefaultAsync(x => x.OrdenId == dto.OrdenId && x.InsumoId == dto.InsumoId);

            if (detalle == null)
                return NotFound("No se encontró el insumo en la orden.");

            // Validar que la cantidad a liberar sea positiva
            if (dto.Cantidad <= 0)
                return BadRequest("Cantidad inválida para liberar.");

            // Actualizar el detalle de la orden: restar lo liberado
            detalle.Cantidad -= dto.Cantidad;

            // Actualizar el stock general de insumos: sumar lo liberado
            var insumo = await context.Insumos.FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo != null)
            {
                insumo.CantDisponible += dto.Cantidad; // Puede quedar positivo o negativo
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"Se liberaron {dto.Cantidad} de {insumo?.Nombre} de la orden {dto.OrdenId}.",
                InsumoActualizado = _mapper.Map<CrearInsumoDTO>(insumo),
                DetalleActualizado = _mapper.Map<InsumoDetalleOrdenDTO>(detalle)
            });
        }

        // DTO para liberar insumos
        public class LiberarInsumoDTO
        {
            public int OrdenId { get; set; }
            public int InsumoId { get; set; }
            public int Cantidad { get; set; }
        }

        [HttpPost("GenerarPedidoPendiente")]
        public async Task<ActionResult> GenerarPedidoPendiente()
        {
            using var conn = context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_GenerarPedidoPendiente";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            using var reader = await cmd.ExecuteReaderAsync();

            if (!reader.Read())
                return BadRequest("No se pudo generar el pedido.");

            var result = new
            {
                PedidoId = Convert.ToInt32(reader["PedidoId"]), // 👈 FIX
                NroPedido = reader.GetInt32(reader.GetOrdinal("NroPedido")),
                Estado = reader.GetString(reader.GetOrdinal("Estado"))
            };

            return Ok(result);
        }

        [HttpPost("GenerarPedidoDetallePorOT")]
        public async Task<IActionResult> GenerarPedidoDetallePorOT(
    int nroOt,
    string nombreInsumo,
    bool insuficiente = true
)
        {
            if (!insuficiente)
                return BadRequest("El insumo no está marcado como insuficiente.");

            // 1️⃣ Buscar InsumoDetalleOrden válido
            var insumoDetalle = await context.InsumoDetalleOrdenes
                .Include(x => x.Insumo)
                .Include(x => x.Orden)
                .Where(x =>
                    x.Orden.NroOT == nroOt &&
                    x.Insumo.Nombre == nombreInsumo &&
                    x.Insuficiente == true &&
                    x.CantidadPendiente > 0
                )
                .FirstOrDefaultAsync();

            if (insumoDetalle == null)
                return BadRequest("No se encontró un insumo insuficiente válido.");

            // 2️⃣ Ejecutar SP usando connection string REAL
            int pedidoId;

            var connectionString = context.Database.GetConnectionString();

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using var cmd = new SqlCommand("sp_GenerarPedidoPendiente", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();

                if (!reader.Read())
                    return BadRequest("No se pudo generar el pedido.");

                pedidoId = Convert.ToInt32(reader["PedidoId"]);
            }

            // 3️⃣ Evitar duplicado
            bool existe = await context.PedidoDetalleInsumos.AnyAsync(x =>
                x.IdPedido == pedidoId &&
                x.Item == insumoDetalle.Insumo.Nombre
            );

            if (existe)
                return BadRequest("El detalle del pedido ya existe.");

            // 4️⃣ Fecha futura automática
            var fechaUso = DateTime.Now.AddDays(Random.Shared.Next(1, 15));

            // 5️⃣ Crear PedidoDetalleInsumo
            var pedidoDetalle = new PedidoDetalleInsumo
            {
                IdPedido = pedidoId,
                IdInsumo = insumoDetalle.InsumoId,
                Item = insumoDetalle.Insumo.Nombre + Environment.NewLine + insumoDetalle.Insumo.Detalle,
                Cantidad = insumoDetalle.CantidadPendiente,
                FechaUso = fechaUso,
                EsSolicitado = false,
                Estado = "Pendiente",
                EntregaParcial = false,
                EntregaTotal = false
            };

            context.PedidoDetalleInsumos.Add(pedidoDetalle);
            await context.SaveChangesAsync();

            // 6️⃣ Respuesta con DTO EXISTENTE
            var response = new PedidoDetalleInsumoDTO
            {
                Id = pedidoDetalle.Id,
                IdPedido = pedidoDetalle.IdPedido,
                IdInsumo = pedidoDetalle.IdInsumo,
                Item = pedidoDetalle.Item,
                Cantidad = pedidoDetalle.Cantidad,
                FechaUso = pedidoDetalle.FechaUso,
                NroOT = nroOt,
                EsSolicitado = false,
                Estado = pedidoDetalle.Estado,
                NombreInsumoInsuficiente = pedidoDetalle.Item,
                EntregaParcial = false,
                EntregaTotal = false
            };

            return Ok(response);
        }







    }
}
