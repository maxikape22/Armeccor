using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;
using System;

namespace Armeccor.Server.Mapeos
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Perfectos para funcionamiento POST y PUT
            CreateMap<CrearClienteDTO, Cliente>();
            CreateMap<CrearOrdenDTO, Orden>();

            //Perfectos para funcionamiento GET y DELETE
            CreateMap<Cliente, CrearClienteDTO>();
            CreateMap<Orden, CrearOrdenDTO>();

            CreateMap<Orden, OrdenDetalleDTO>()
                .ForMember(dest => dest.NombreCliente, opt => opt
                .MapFrom(src => src.Cliente.Nombre));

            CreateMap<CrearAreaDTO, Area>()
                .ForMember(x => x.NombreArea, y => y
                .MapFrom(s => s.NombreArea))
                .ReverseMap();

            CreateMap<Area,AreaListaDTO>()
                .ForMember(dest => dest.NombreArea, opt => opt
                .MapFrom(src => src.NombreArea))
                .ReverseMap();

            CreateMap<CrearEntregaDTO, Entrega>()
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.IdOrden)).ReverseMap();

            CreateMap<Entrega, EntregaDetalleDTO>()
                .ForMember(dest => dest.FechaEntrega, opt => opt
                .MapFrom(src =>src.Orden != null ? src.Orden.FechaEntrega : DateTime.MinValue))
                .ReverseMap();

            CreateMap<CrearInsumoDTO, Insumo>()
                .ForMember(x=>x.Nombre,m=>m
                .MapFrom(d=>d.Nombre))
                .ForMember(n=>n.CantDisponible,b=>b
                .MapFrom(p=>p.CantDisponible)).ReverseMap();

            CreateMap<Insumo, CrearInsumoDTO>()
                .ForMember(dest => dest.CantDisponible, opt => opt
                .MapFrom(src => src.CantDisponible))
                .ForMember(dest => dest.Nombre, opt => opt
                .MapFrom(src => src.Nombre))
                .ReverseMap();

            CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenDTO>()
                .ForMember(dest => dest.InsumoId, opt => opt
                .MapFrom(src => src.InsumoId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Cantidad, opt => opt
                .MapFrom(src => src.Cantidad));

            CreateMap<InsumoDetalleOrdenDTO, InsumoDetalleOrden>()
                .ForMember(dest => dest.InsumoId, opt => opt
                .MapFrom(src => src.InsumoId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Cantidad, opt => opt
                .MapFrom(src => src.Cantidad));

            CreateMap<InsumoDetalleOrden, InsumoDetalleOrdenListaDTO>()
                .ForMember(c=>c.Id, d=>d
                .MapFrom(f=>f.Id))
                .ForMember(k=>k.InsumoId, l=>l
                .MapFrom(w=>w.InsumoId))
                .ForMember(e=>e.OrdenId,t=>t
                .MapFrom(q=>q.OrdenId))
                .ForMember(f=>f.Cantidad,a=>a
                .MapFrom(ñ=>ñ.Cantidad));

            CreateMap<InsumoDetalleOrdenListaDTO, InsumoDetalleOrden>()
                .ForMember(c=>c.Id, d=>d
                .MapFrom(f=>f.Id))
                .ForMember(k=>k.InsumoId, l=>l
                .MapFrom(w=>w.InsumoId))
                .ForMember(e=>e.OrdenId,t=>t
                .MapFrom(q=>q.OrdenId))
                .ForMember(f=>f.Cantidad,a=>a
                .MapFrom(ñ=>ñ.Cantidad));

            CreateMap<AreaDetalleOrden, AreaDetalleOrdenListaDTO>()
                .ForMember(dest => dest.Id, opt => opt
                .MapFrom(src => src.Id))
                .ForMember(dest => dest.AreaId, opt => opt
                .MapFrom(src => src.AreaId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Descripcion, opt => opt
                .MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Estado, opt => opt
                .MapFrom(src => src.Estado))
                .ForMember(dest => dest.Tiempo, opt => opt
                .MapFrom(src => src.Tiempo));

            CreateMap<AreaDetalleOrdenListaDTO, AreaDetalleOrden>()
                .ForMember(dest => dest.Id, opt => opt
                .MapFrom(src => src.Id))
                .ForMember(dest => dest.AreaId, opt => opt
                .MapFrom(src => src.AreaId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Descripcion, opt => opt
                .MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Estado, opt => opt
                .MapFrom(src => src.Estado))
                .ForMember(dest => dest.Tiempo, opt => opt
                .MapFrom(src => src.Tiempo));

            CreateMap<AreaDetalleOrden, AreaDetalleOrdenDTO>()
                .ForMember(dest => dest.AreaId, opt => opt
                .MapFrom(src => src.AreaId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Descripcion, opt => opt
                .MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Estado, opt => opt
                .MapFrom(src => src.Estado))
                .ForMember(dest => dest.Tiempo, opt => opt
                .MapFrom(src => src.Tiempo));

            CreateMap<AreaDetalleOrdenDTO, AreaDetalleOrden>()
                .ForMember(dest => dest.AreaId, opt => opt
                .MapFrom(src => src.AreaId))
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.OrdenId))
                .ForMember(dest => dest.Descripcion, opt => opt
                .MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Estado, opt => opt
                .MapFrom(src => src.Estado))
                .ForMember(dest => dest.Tiempo, opt => opt
                .MapFrom(src => src.Tiempo));
        }
    }
}