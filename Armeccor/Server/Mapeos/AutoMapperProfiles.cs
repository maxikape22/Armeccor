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
                .ForMember(dest => dest.Planos, opt => opt.Ignore());

            CreateMap<Orden, OrdenConAreasDTO>();

            CreateMap<OrdenDTO, Orden>()
                .ForMember(dest => dest.AreaDetalleOrdenes, opt => opt.Ignore())
                .ForMember(dest => dest.Entregas, opt => opt.Ignore())
                .ForMember(dest => dest.InsumoOrdenes, opt => opt.Ignore())
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Planos, opt => opt.Ignore());

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
                .ForMember(dest => dest.NombreOrden,
                    opt => opt
                    .MapFrom(src => src.Orden.NombreOrden))
                .ForMember(dest => dest.FechaEntrega,
                    opt => opt
                    .MapFrom(src => src.Orden != null ? src.Orden.FechaEntrega : DateTime.MinValue))
                .ForMember(dest => dest.NroOT,
                    opt => opt
                    .MapFrom(src => src.Orden.NroOT))
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

            CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenDTO>()
                .ForMember(dest => dest.Nombre, opt => opt
                .MapFrom(src => src.Insumo.Nombre)); // <-- ¡CLAVE!

            // ================== AREA DETALLE ORDEN ==================
            CreateMap<AreaDetalleOrden, AreaDetalleOrdenListaDTO>()  
                .ForMember(dest => dest.NombreArea, opt => opt
                .MapFrom(src => src.Area.NombreArea))
                .ForMember(d=>d.NombreOrden,p=>p
                .MapFrom(d=>d.Orden.NombreOrden))
                .ForMember(c=>c.NombreCliente,m=>m
                .MapFrom(e=>e.Orden.Cliente.Nombre)).ReverseMap();

            CreateMap<AreaDetalleOrdenListaDTO, AreaDetalleOrden>();

            CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>().ReverseMap();


            //CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>()
            //    .ForMember(d => d.NombreOrden, o => o
            //    .MapFrom(e=>e.Orden.NombreOrden))
            //    .ForMember(l=>l.NombreCliente,ñ=>ñ
            //    .MapFrom(q=>q.Orden.Cliente.Nombre));

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
            //CreateMap<PlanoFiltroDTO, Plano>().ReverseMap();

            CreateMap<Plano, PlanoFiltroDTO>()
            .ForMember(dest => dest.NroOT, opt => opt.Ignore())
            .ForMember(dest => dest.NombreOrden, opt => opt.Ignore()).ReverseMap();

            // ================== PEDIDO ==================

            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.Nombre, opt => opt
                .MapFrom(src => src.Proveedor.Nombre));

            CreateMap<PedidoDTO, Pedido>()
                .ForMember(dest => dest.Proveedor, opt => opt.MapFrom(d => d.Nombre));


            // ================== PEDIDO DETALLE INSUMO ==================

            CreateMap<PedidoDetalleInsumo, PedidoDetalleInsumoDTO>()
                .ForMember(dest => dest.IdPedido, opt => opt
                .Ignore()).ForMember(dest => dest.IdInsumo, opt => opt.Ignore())
                .ForMember(c => c.NroOT, i => i
                .MapFrom(d => d.Insumo.InsumoOrdenes
                .Select(x => x.Orden.NroOT).FirstOrDefault()))
                .ForMember(d=>d.NombreInsumoInsuficiente,p=>p
                .MapFrom(d=>d.Insumo.Nombre))
                .ReverseMap();

            // ================== PROVEEDOR ==================

            CreateMap<Proveedor, ProveedorDTO>().ReverseMap();

        }
    } 
}
