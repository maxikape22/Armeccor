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
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area.NombreArea));

            CreateMap<CrearAreaDTO, Area>()
                .ForMember(x => x.NombreArea, y => y
                .MapFrom(s => s.NombreArea))
                .ForMember(h => h.Tiempo, l => l
                .MapFrom(ñ => ñ.Tiempo))
                .ForMember(e => e.Estado, q => q
                .MapFrom(h => h.Estado))
                .ReverseMap();

            CreateMap<Area,AreaListaDTO>()
                .ForMember(dest => dest.NombreArea, opt => opt
                .MapFrom(src => src.NombreArea))
                .ForMember(dest => dest.Tiempo, opt => opt.
                MapFrom(src => src.Tiempo))
                .ForMember(dest => dest.Estado, opt => opt
                .MapFrom(src => src.Estado))
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

            CreateMap<InsumoOrden, InsumoOrdenDTO>().ReverseMap();
            CreateMap<InsumoOrdenDTO, InsumoOrden>().ReverseMap();

            CreateMap<InsumoOrden, InsumoEnOrdenVistaDTO>().ReverseMap();
            CreateMap<InsumoEnOrdenVistaDTO, InsumoOrden>().ReverseMap();
        }
    }
}