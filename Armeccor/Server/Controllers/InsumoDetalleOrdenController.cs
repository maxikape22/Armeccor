using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public async Task<ActionResult<object>> PostCliente(InsumoDetalleOrdenDTO dto)
        {
            var insumo = await context.Insumos
                .FirstOrDefaultAsync(x => x.Id == dto.InsumoId);
            if (insumo == null)
                return NotFound("Insumo no encontrado");

            // ✅ ACÁ
            //if (!insumo.EstaActivo)
            //{
                insumo.EstaActivo = true;
                insumo.FechaBorrado = null;
            //}
            // ✅ FIN ACÁ

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

                // 🔴 TAMBIÉN ACÁ
                detalle.EstaActivo = true;
                detalle.FechaBaja = null;
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
                    Insuficiente = pendienteAhora > 0,
                    EstaActivo = true,
                    FechaBaja = null
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

        [HttpGet("AgrupadosPorOrden/{NroOT:int}")]
        public async Task<ActionResult<List<InsumoDetalleOrdenDTO>>>GetInsumosAgrupadosPorOrden(int NroOT)
        {
            var lista = await context.InsumoDetalleOrdenes                    
                .Where(d => d.EstaActivo == true)
                .Include(x => x.Insumo)                   
                .Where(x => x.Orden.NroOT == NroOT)
                .Select(x => new InsumoDetalleOrdenDTO
                {
                    Id = x.Id,
                    InsumoId = x.InsumoId.Value,
                    OrdenId = x.OrdenId.Value,
                    Nombre = x.Insumo.Nombre,
                    NroOT = x.Orden.NroOT,
                    // 🔹 valores REALES persistidos
                    Cantidad = x.Cantidad,
                    CantidadDescontada = x.CantidadDescontada,
                    CantidadPendiente = x.CantidadPendiente,

                    // 🔹 estado DERIVADO (fuente de verdad)
                    Insuficiente = x.CantidadPendiente > 0,
                    EstaActivo = x.EstaActivo,
                    FechaBaja = x.FechaBaja
                })
                .ToListAsync();

            return Ok(lista);
        }

        //Metodo para liberar los insumos que tienen cargados una orden ,
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
                    x.EstaActivo == true &&          // 🔴 CLAVE
                    x.Orden.NroOT == nroOt &&
                    x.Insumo.Nombre == nombreInsumo &&
                    x.Insuficiente == true &&
                    x.CantidadPendiente > 0
                )
                .FirstOrDefaultAsync();


            if (insumoDetalle == null)
                return BadRequest("No se encontró un insumo insuficiente válido.");

            // ✅ ACÁ VA — JUSTO ACÁ
            if (!insumoDetalle.Insumo.EstaActivo)
                return BadRequest("El insumo está dado de baja.");


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
                EntregaTotal = false,
                EstaActivo = true
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
                EntregaTotal = false,
                EstaActivo = true
            };

            return Ok(response);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLogicoInsumoDetallado(int id)
        {
            var insumoDetalle = await context.InsumoDetalleOrdenes.FirstOrDefaultAsync(e => e.Id == id);

            if (insumoDetalle == null)
                return NotFound("Insumo no encontrado.");

            if (!insumoDetalle.EstaActivo)
                return BadRequest("El insumo fue dado de baja.");

            insumoDetalle.EstaActivo = false;
            insumoDetalle.FechaBaja = DateTime.Now;

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
