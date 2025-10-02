using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using System;
using System.Linq;

namespace Armeccor.Server.Mapeos
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region MyRegion
            ////Perfectos para funcionamiento POST y PUT
            //CreateMap<CrearClienteDTO, Cliente>();
            //CreateMap<CrearOrdenDTO, Orden>();

            ////Perfectos para funcionamiento GET y DELETE
            //CreateMap<Cliente, CrearClienteDTO>();
            //CreateMap<Orden, CrearOrdenDTO>();

            //CreateMap<Orden, OrdenDetalleDTO>()
            //    .ForMember(dest => dest.NombreCliente, opt => opt
            //    .MapFrom(src => src.Cliente.Nombre));

            //CreateMap<CrearAreaDTO, Area>()
            //    .ForMember(x => x.NombreArea, y => y
            //    .MapFrom(s => s.NombreArea))
            //    .ReverseMap();

            //CreateMap<Area, AreaListaDTO>()
            //    .ForMember(dest => dest.NombreArea, opt => opt
            //    .MapFrom(src => src.NombreArea))
            //    .ReverseMap();

            //CreateMap<CrearEntregaDTO, Entrega>()
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.IdOrden)).ReverseMap();

            //CreateMap<Entrega, EntregaDetalleDTO>()
            //    .ForMember(dest => dest.FechaEntrega, opt => opt
            //    .MapFrom(src => src.Orden != null ? src.Orden.FechaEntrega : DateTime.MinValue))
            //    .ReverseMap();

            //CreateMap<CrearInsumoDTO, Insumo>()
            //    .ForMember(x => x.Nombre, m => m
            //    .MapFrom(d => d.Nombre))
            //    .ForMember(n => n.CantDisponible, b => b
            //    .MapFrom(p => p.CantDisponible)).ReverseMap();

            //CreateMap<Insumo, CrearInsumoDTO>()
            //    .ForMember(dest => dest.CantDisponible, opt => opt
            //    .MapFrom(src => src.CantDisponible))
            //    .ForMember(dest => dest.Nombre, opt => opt
            //    .MapFrom(src => src.Nombre))
            //    .ReverseMap();

            //CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenDTO>()
            //    .ForMember(dest => dest.InsumoId, opt => opt
            //    .MapFrom(src => src.InsumoId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Cantidad, opt => opt
            //    .MapFrom(src => src.Cantidad));

            //CreateMap<InsumoDetalleOrdenDTO, InsumoDetalleOrden>()
            //    .ForMember(dest => dest.InsumoId, opt => opt
            //    .MapFrom(src => src.InsumoId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Cantidad, opt => opt
            //    .MapFrom(src => src.Cantidad));

            //CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenListaDTO>()
            //    .ForMember(c => c.Id, d => d
            //    .MapFrom(f => f.Id))
            //    .ForMember(k => k.InsumoId, l => l
            //    .MapFrom(w => w.InsumoId))
            //    .ForMember(e => e.OrdenId, t => t
            //    .MapFrom(q => q.OrdenId))
            //    .ForMember(f => f.Cantidad, a => a
            //    .MapFrom(ñ => ñ.Cantidad));

            //CreateMap<InsumoDetalleOrdenListaDTO, InsumoDetalleOrden>()
            //    .ForMember(c => c.Id, d => d
            //    .MapFrom(f => f.Id))
            //    .ForMember(k => k.InsumoId, l => l
            //    .MapFrom(w => w.InsumoId))
            //    .ForMember(e => e.OrdenId, t => t
            //    .MapFrom(q => q.OrdenId))
            //    .ForMember(f => f.Cantidad, a => a
            //    .MapFrom(ñ => ñ.Cantidad));

            //CreateMap<AreaDetalleOrden, AreaDetalleOrdenListaDTO>()
            //    .ForMember(dest => dest.Id, opt => opt
            //    .MapFrom(src => src.Id))
            //    .ForMember(dest => dest.AreaId, opt => opt
            //    .MapFrom(src => src.AreaId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Descripcion, opt => opt
            //    .MapFrom(src => src.Descripcion))
            //    .ForMember(dest => dest.Estado, opt => opt
            //    .MapFrom(src => src.Estado))
            //    .ForMember(dest => dest.Tiempo, opt => opt
            //    .MapFrom(src => src.Tiempo))
            //    .ForMember(dest => dest.NombreArea, opt => opt
            //    .MapFrom(src => src.Area.NombreArea));

            //CreateMap<AreaDetalleOrdenListaDTO, AreaDetalleOrden>()
            //    .ForMember(dest => dest.Id, opt => opt
            //    .MapFrom(src => src.Id))
            //    .ForMember(dest => dest.AreaId, opt => opt
            //    .MapFrom(src => src.AreaId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Descripcion, opt => opt
            //    .MapFrom(src => src.Descripcion))
            //    .ForMember(dest => dest.Estado, opt => opt
            //    .MapFrom(src => src.Estado))
            //    .ForMember(dest => dest.Tiempo, opt => opt
            //    .MapFrom(src => src.Tiempo));

            //CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>()
            //    .ForMember(dest => dest.AreaId, opt => opt
            //    .MapFrom(src => src.AreaId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Descripcion, opt => opt
            //    .MapFrom(src => src.Descripcion))
            //    .ForMember(dest => dest.Estado, opt => opt
            //    .MapFrom(src => src.Estado))
            //    .ForMember(dest => dest.Tiempo, opt => opt
            //    .MapFrom(src => src.Tiempo));

            //CreateMap<AreaDetalleOrdenDTO, AreaDetalleOrden>()
            //    .ForMember(dest => dest.AreaId, opt => opt
            //    .MapFrom(src => src.AreaId))
            //    .ForMember(dest => dest.OrdenId, opt => opt
            //    .MapFrom(src => src.OrdenId))
            //    .ForMember(dest => dest.Descripcion, opt => opt
            //    .MapFrom(src => src.Descripcion))
            //    .ForMember(dest => dest.Estado, opt => opt
            //    .MapFrom(src => src.Estado))
            //    .ForMember(dest => dest.Tiempo, opt => opt
            //    .MapFrom(src => src.Tiempo));

            //// Mapea las propiedades de OrdenConAreasDTO a la entidad Orden
            //CreateMap<OrdenConAreasDTO, Orden>()
            //    .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.Ignore())
            //    .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            //    .ForMember(dest => dest.Plano, opt => opt.Ignore());

            //// También necesitarás un mapeo para la dirección inversa si lo usas
            //CreateMap<Orden, OrdenConAreasDTO>();

            //// Agrega aquí los demás mapeos que ya tengas
            //CreateMap<Area, AreaDTO>().ReverseMap();
            //// ... otros mapeos ...



            ////MAS MAPEOS AL PEDO PROBABLEMENTE

            //// Mapeo para la entidad Area y su DTO simplificado para la lista de áreas (checkboxes)
            //CreateMap<Area, AreaListaDTO>().ReverseMap();

            //// Mapeo para la entidad Orden y su DTO para guardar los datos completos
            //// Ignoramos las listas de relaciones ya que la lógica la manejas en el controlador.
            //CreateMap<OrdenDTO, Orden>()
            //    .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.Ignore())
            //    .ForMember(dest => dest.Entregas, opt => opt.Ignore())
            //    .ForMember(dest => dest.InsumoOrdenes, opt => opt.Ignore())
            //    .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            //    .ForMember(dest => dest.Plano, opt => opt.Ignore());

            //// Mapeo para Orden a DTO (para obtener los datos)
            //CreateMap<Orden, OrdenDTO>()
            //    .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.MapFrom(src => src.AreaDetalleOrdenes));

            //// Mapeo para la entidad intermedia AreaDetalleOrden y su DTO
            //// Mapea la propiedad 'area' para incluir el nombre del área.
            //CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>()
            //    .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId))
            //    .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.Area));

            //// Mapeo inverso, para guardar.
            //CreateMap<AreaDetalleOrdenDTO, AreaDetalleOrden>()
            //    .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId));

            //// Mapeo para la entidad Area y el DTO anidado
            //CreateMap<Area, AreaDTO>().ReverseMap();

            //CreateMap<AreaDetalleOrden, RegistrarAreaDetallarDTO>().ReverseMap();
            //CreateMap<RegistrarAreaDetallarDTO, AreaDetalleOrden>().ReverseMap();

            //// Mapear de DTO a entidad
            //CreateMap<AreaDetalleOrdenCrearDTO, AreaDetalleOrden>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())          // Id es Identity
            //    .ForMember(dest => dest.OrdenId, opt => opt.Ignore())    // Se asigna en el backend
            //    .ForMember(dest => dest.AreaId, opt => opt.Ignore());    // Se asigna en el backend

            //// Mapear de entidad a DTO de lista (si necesitas mostrar después)
            //CreateMap<AreaDetalleOrden, AreaDetalleOrdenListaDTO>()
            //    .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area.NombreArea));
            #endregion


            //SUPUESTAMENTE ESTOS MAPEOS ESTAN LIMPIOS DE ERRORES, REDUNDANCIA Y REPETICIONES
            //--> SI NO FUNCIONA, VOLVER A LOS DE ARRIBA

            // ================== CLIENTE ==================
            CreateMap<CrearClienteDTO, Cliente>().ReverseMap();
            CreateMap<Cliente, CrearClienteDTO>().ReverseMap();

            // ================== ORDEN ==================
            CreateMap<CrearOrdenDTO, Orden>().ReverseMap();

            CreateMap<Orden, OrdenDetalleDTO>()
                .ForMember(dest => dest.NombreCliente,
                    opt => opt.MapFrom(src => src.Cliente.Nombre));

            CreateMap<OrdenConAreasDTO, Orden>()
                .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Plano, opt => opt.Ignore());

            CreateMap<Orden, OrdenConAreasDTO>();

            CreateMap<OrdenDTO, Orden>()
                .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.Ignore())
                .ForMember(dest => dest.Entregas, opt => opt.Ignore())
                .ForMember(dest => dest.InsumoOrdenes, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Plano, opt => opt.Ignore());

            CreateMap<Orden, OrdenDTO>()
                .ForMember(dest => dest.AreaDetalleOrdenes,
                    opt => opt.MapFrom(src => src.AreaDetalleOrdenes));

            //CreateMap<Orden, OrdenDetalleDTO>()
            //    .ForMember(dest => dest.AreaActual, opt => opt.MapFrom(src => src.AreaDetalleOrdenes
            //    .Where(ado => ado.Estado == "Iniciado")
            //    .Select(ado => ado.Area.NombreArea)
            //    .FirstOrDefault()));

            CreateMap<Orden, OrdenDetalleDTO>()
                .ForMember(dest => dest.AreaActual, opt => opt.MapFrom(src =>
            src.AreaDetalleOrdenes.FirstOrDefault(ado => ado.Estado == "Iniciado") != null
            ? src.AreaDetalleOrdenes.FirstOrDefault(ado => ado.Estado == "Iniciado").Area.NombreArea
            : "N/A"))
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre));

            // ================== AREA ==================
            CreateMap<CrearAreaDTO, Area>().ReverseMap();
            CreateMap<Area, AreaListaDTO>().ReverseMap();
            CreateMap<Area, AreaDTO>().ReverseMap();

            // ================== ENTREGA ==================
            CreateMap<CrearEntregaDTO, Entrega>()
                .ForMember(dest => dest.OrdenId, opt => opt.MapFrom(src => src.IdOrden))
                .ReverseMap();

            CreateMap<Entrega, EntregaDetalleDTO>()
                .ForMember(dest => dest.FechaEntrega,
                    opt => opt
                    .MapFrom(src => src.Orden != null ? src.Orden.FechaEntrega : DateTime.MinValue))
                .ForMember(dest => dest.MedioDePago,
                   opt => opt.
                   MapFrom(src => src.Medio_De_Pago != null ? src.Medio_De_Pago.Nombre_Medio : string.Empty))
                .ReverseMap();

            // Para mostrar Entregas (DTO con Nombre del medio)
            CreateMap<Entrega, CrearEntregaDTO>()
                .ForMember(dest => dest.MedioDePagoId, opt => opt.MapFrom(src => src.Medio_De_Pago.Nombre_Medio));


            //Esta configuracion hace que la fecha de entrega salga como 01/01/0001
            //CreateMap<Entrega, EntregaDetalleDTO>()
                //.ForMember(dest => dest.MedioDePago,
                //   opt => opt.
                //   MapFrom(src => src.Medio_De_Pago != null ? src.Medio_De_Pago.Nombre_Medio : string.Empty));

            // Para crear nuevas Entregas (se recibe MedioDePagoId)
            CreateMap<CrearEntregaDTO, Entrega>()
                .ForMember(dest => dest.MedioDePagoId, opt => opt.MapFrom(src => src.MedioDePagoId));


            // ================== INSUMO ==================
            CreateMap<CrearInsumoDTO, Insumo>().ReverseMap();
            CreateMap<Insumo, CrearInsumoDTO>().ReverseMap();

            // ================== INSUMO DETALLE ORDEN ==================
            CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenDTO>().ReverseMap();
            CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenListaDTO>().ReverseMap();

            // ================== AREA DETALLE ORDEN ==================
            CreateMap<AreaDetalleOrden, AreaDetalleOrdenListaDTO>()
                .ForMember(dest => dest.NombreArea,
                    opt => opt.MapFrom(src => src.Area.NombreArea));

            CreateMap<AreaDetalleOrdenListaDTO, AreaDetalleOrden>();

            CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>().ReverseMap();

            CreateMap<AreaDetalleOrden, RegistrarAreaDetallarDTO>().ReverseMap();

            CreateMap<AreaDetalleOrdenCrearDTO, AreaDetalleOrden>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrdenId, opt => opt.Ignore())
                .ForMember(dest => dest.AreaId, opt => opt.Ignore());

            // ================== MEDIO DE PAGO ==================

            CreateMap<MedioDePago, MedioDePagoDTO>().ForMember(x => x.Nombre_Medio, y => y.MapFrom(z => z.Nombre_Medio));
            CreateMap<MedioDePagoDTO, MedioDePago>().ForMember(x => x.Nombre_Medio, y => y.MapFrom(z => z.Nombre_Medio));

            // ================== NOTIFICACIÓN ==================

            CreateMap<Notificacion, NotificacionDTO>();
            CreateMap<NotificacionDTO, Notificacion>();
        }
    } 
}
