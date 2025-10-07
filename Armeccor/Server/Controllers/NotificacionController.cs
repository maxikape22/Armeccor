using Armeccor.Datos;
using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Armeccor.API.Controllers
{
    [ApiController]
    [Route("api/Notificaciones")]
    public class NotificacionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificacionController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region
        //[HttpGet]
        //public async Task<ActionResult<List<NotificacionDTO>>> Get()
        //{
        //    var notificaciones = new List<Notificacion>();

        //    // 🔍 Insumos críticos
        //    var insumosCriticos = await _context.Insumos
        //        .Where(i => i.CantDisponible <= 10)
        //        .Select(i => new Notificacion
        //        {
        //            Titulo = "Stock crítico",
        //            Mensaje = $"El insumo '{i.Nombre}' tiene solo {i.CantDisponible} unidades.",
        //            Tipo = "Insumo",
        //            Fecha = DateTime.Now,
        //            EsLeida = false,
        //            InsumoId = i.Id
        //        }).ToListAsync();

        //    notificaciones.AddRange(insumosCriticos);

        //    // 📦 Órdenes finalizadas
        //    var ordenesFinalizadas = await _context.Ordenes
        //        .Where(o => o.Estado == "Finalizada")
        //        .Select(o => new Notificacion
        //        {
        //            Titulo = "Orden finalizada",
        //            Mensaje = $"La orden '{o.NroOT}' ha finalizado.",
        //            Tipo = "Orden",
        //            Fecha = DateTime.Now,
        //            EsLeida = false,
        //            OrdenId = o.Id
        //        }).ToListAsync();

        //    notificaciones.AddRange(ordenesFinalizadas);

        //    // 🧩 Áreas finalizadas dentro de órdenes
        //    var areasFinalizadas = await _context.AreaDetalleOrdenes
        //        .Where(ad => ad.Estado == "Finalizado")
        //        .Select(ad => new Notificacion
        //        {
        //            Titulo = "Área finalizada",
        //            //Mensaje = $"Área #{ad.AreaId} finalizada en orden #{ad.OrdenId}.",
        //            Mensaje = $"El área ha finalizado",
        //            Tipo = $"{ad.Area.NombreArea}",
        //            Fecha = DateTime.Now,
        //            EsLeida = false,
        //            AreaDetalleId = ad.Id
        //        }).ToListAsync();

        //    notificaciones.AddRange(areasFinalizadas);

        //    var dtoList = _mapper.Map<List<NotificacionDTO>>(notificaciones.OrderByDescending(n => n.Fecha));
        //    return Ok(dtoList);
        //}
        #endregion

        #region
        //[HttpGet]
        //public async Task<ActionResult<List<NotificacionDTO>>> Get()
        //{
        //    var notificaciones = new List<Notificacion>();

        //    // 🔍 Insumos críticos
        //    var insumosCriticos = await _context.Insumos
        //        .Where(i => i.CantDisponible <= 10)
        //        .ToListAsync();

        //    foreach (var i in insumosCriticos)
        //    {
        //        bool existe = await _context.Notificaciones.AnyAsync(n =>
        //            n.InsumoId == i.Id && n.Tipo == "Insumo");

        //        if (!existe)
        //        {
        //            var nueva = new Notificacion
        //            {
        //                Titulo = "Stock crítico",
        //                Mensaje = $"El insumo '{i.Nombre}' tiene solo {i.CantDisponible} unidades.",
        //                Tipo = "Insumo",
        //                Fecha = DateTime.Now,
        //                EsLeida = false,
        //                InsumoId = i.Id
        //            };

        //            _context.Notificaciones.Add(nueva);
        //            notificaciones.Add(nueva);
        //        }
        //    }

        //    // 📦 Órdenes finalizadas
        //    var ordenesFinalizadas = await _context.Ordenes
        //        .Where(o => o.Estado == "Finalizada")
        //        .ToListAsync();

        //    foreach (var o in ordenesFinalizadas)
        //    {
        //        bool existe = await _context.Notificaciones.AnyAsync(n =>
        //            n.OrdenId == o.Id && n.Tipo == "Orden");

        //        if (!existe)
        //        {
        //            var nueva = new Notificacion
        //            {
        //                Titulo = "Orden finalizada",
        //                Mensaje = $"La orden '{o.NroOT}' ha finalizado.",
        //                Tipo = "Orden",
        //                Fecha = DateTime.Now,
        //                EsLeida = false,
        //                OrdenId = o.Id
        //            };

        //            _context.Notificaciones.Add(nueva);
        //            notificaciones.Add(nueva);
        //        }
        //    }

        //    // 🧩 Áreas finalizadas dentro de órdenes
        //    var areasFinalizadas = await _context.AreaDetalleOrdenes
        //        .Include(ad => ad.Area)
        //        .Where(ad => ad.Estado == "Finalizado")
        //        .ToListAsync();

        //    foreach (var ad in areasFinalizadas)
        //    {
        //        bool existe = await _context.Notificaciones.AnyAsync(n =>
        //            n.AreaDetalleId == ad.Id && n.Tipo == ad.Area.NombreArea);

        //        if (!existe)
        //        {
        //            var nueva = new Notificacion
        //            {
        //                Titulo = "Área finalizada",
        //                Mensaje = $"El área ha finalizado",
        //                Tipo = ad.Area.NombreArea,
        //                Fecha = DateTime.Now,
        //                EsLeida = false,
        //                AreaDetalleId = ad.Id
        //            };

        //            _context.Notificaciones.Add(nueva);
        //            notificaciones.Add(nueva);
        //        }
        //    }

        //    await _context.SaveChangesAsync();

        //    var todas = await _context.Notificaciones
        //        .OrderByDescending(n => n.Fecha)
        //        .ToListAsync();

        //    var dtoList = _mapper.Map<List<NotificacionDTO>>(todas);
        //    return Ok(dtoList);
        //}

        #endregion

        [HttpGet]
        public async Task<ActionResult<List<NotificacionDTO>>> Get()
        {
            var notificaciones = new List<Notificacion>();

            // 🔍 Insumos críticos
            var insumosCriticos = await _context.Insumos
                .Where(i => i.CantDisponible <= 10)
                .ToListAsync();

            foreach (var i in insumosCriticos)
            {
                bool yaNotificado = await _context.EventosNotificados.AnyAsync(e =>
                    e.Tipo == "Insumo" && e.ReferenciaId == i.Id);

                if (!yaNotificado)
                {
                    var nueva = new Notificacion
                    {
                        Titulo = "Stock crítico",
                        Mensaje = $"El insumo '{i.Nombre}' tiene solo {i.CantDisponible} unidades.",
                        Tipo = "Insumo",
                        Fecha = DateTime.Now.Date,
                        EsLeida = false,
                        //InsumoId = i.Id
                    };

                    _context.Notificaciones.Add(nueva);
                    _context.EventosNotificados.Add(new EventoNotificado
                    {
                        Tipo = "Insumo",
                        ReferenciaId = i.Id,
                        Fecha = DateTime.Now.Date
                    });

                    notificaciones.Add(nueva);
                }
            }

            // 📦 Órdenes finalizadas
            var ordenesFinalizadas = await _context.Ordenes
                .Where(o => o.Estado == "Finalizada")
                .ToListAsync();

            foreach (var o in ordenesFinalizadas)
            {
                bool yaNotificado = await _context.EventosNotificados.AnyAsync(e =>
                    e.Tipo == "Orden" && e.ReferenciaId == o.Id);

                if (!yaNotificado)
                {
                    var nueva = new Notificacion
                    {
                        Titulo = "Orden finalizada",
                        Mensaje = $"La orden '{o.NroOT}' ha finalizado.",
                        Tipo = "Orden",
                        Fecha = DateTime.Now.Date,
                        EsLeida = false,
                        //OrdenId = o.Id
                    };

                    _context.Notificaciones.Add(nueva);
                    _context.EventosNotificados.Add(new EventoNotificado
                    {
                        Tipo = "Orden",
                        ReferenciaId = o.Id,
                        Fecha = DateTime.Now.Date
                    });

                    notificaciones.Add(nueva);
                }
            }

            // 🧩 Áreas finalizadas dentro de órdenes
            var areasFinalizadas = await _context.AreaDetalleOrdenes
                .Include(ad => ad.Area)
                .Where(ad => ad.Estado == "Finalizado")
                .ToListAsync();

            foreach (var ad in areasFinalizadas)
            {
                bool yaNotificado = await _context.EventosNotificados.AnyAsync(e =>
                    e.Tipo == ad.Area.NombreArea && e.ReferenciaId == ad.Id);

                if (!yaNotificado)
                {
                    var nueva = new Notificacion
                    {
                        Titulo = "Área finalizada",
                        Mensaje = $"El área ha finalizado",
                        Tipo = ad.Area.NombreArea,
                        Fecha = DateTime.Now.Date   ,
                        EsLeida = false,
                       // AreaDetalleId = ad.Id
                    };

                    _context.Notificaciones.Add(nueva);
                    _context.EventosNotificados.Add(new EventoNotificado
                    {
                        Tipo = ad.Area.NombreArea,
                        ReferenciaId = ad.Id,
                        Fecha = DateTime.Now.Date
                    });

                    notificaciones.Add(nueva);
                }
            }

            await _context.SaveChangesAsync();

            var todas = await _context.Notificaciones
                .OrderByDescending(n => n.Fecha)
                .ToListAsync();

            var dtoList = _mapper.Map<List<NotificacionDTO>>(todas);
            return Ok(dtoList);
        }


        [HttpPut("marcar-leida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            var notif = await _context.Notificaciones.FindAsync(id);
            if (notif == null) return NotFound();

            notif.EsLeida = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("marcar-todas-leidas")]
        public async Task<IActionResult> MarcarTodasComoLeidas()
        {
            var noLeidas = await _context.Notificaciones
                .Where(n => !n.EsLeida)
                .ToListAsync();

            foreach (var notif in noLeidas)
            {
                notif.EsLeida = true;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("limpiar")]
        public async Task<IActionResult> LimpiarTodas()
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Notificaciones;");
            return NoContent();
        }
    }
}