using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;

namespace Armeccor.Server.Mapeos
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //Perfectos para funcionamiento POST y PUT
            CreateMap<CrearClienteDTO, Cliente>();
            CreateMap<CrearOrdenDTO, Orden>();

            //Perfectos para funcionamiento GET y DELETE
            CreateMap<Cliente, CrearClienteDTO>();
            CreateMap<Orden, CrearOrdenDTO>();

            //Mapeos para funcionamiento del formulario de Ordenes

            CreateMap<Orden, OrdenDetalleDTO>()
            .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre)); // Mapea el nombre del cliente
                                                                                                   // .ForMember para otras propiedades anidadas si las necesitas aplanadas
                                                                                                   // Asegúrate de mapear también las colecciones:
                                                                                                   // .ForMember(dest => dest.AreasDeLaOrden, opt => opt.MapFrom(src => src.AreaOrdenes));
                                                                                                   // ... y así sucesivamente para Piezas, Entregas, InsumosOrden

            CreateMap<AreaOrden, AreaOrdenDetalleDTO>()
                .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area.NombreArea));

            CreateMap<Pieza, PiezaDetalleDTO>();
            CreateMap<Entrega, EntregaDetalleDTO>();
            CreateMap<InsumoOrden, InsumoOrdenDetalleDTO>()
                .ForMember(dest => dest.NombreInsumo, opt => opt.MapFrom(src => src.Insumo.Nombre));

            // Mapeo inverso de OrdenDetalleDTO a CrearOrdenDTO para el PUT (si lo necesitas)
            CreateMap<OrdenDetalleDTO, CrearOrdenDTO>();
        }
    }
}