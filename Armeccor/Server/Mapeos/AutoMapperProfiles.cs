using Armeccor.Datos.Entidades;
using AutoMapper;
using DTO.ObjetosDTO;

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

            //Mapeos para funcionamiento del formulario de Ordenes

            //CreateMap<Orden, OrdenDetalleDTO>()
            //.ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre)); // Mapea el nombre del cliente
                                                                                                   // .ForMember para otras propiedades anidadas si las necesitas aplanadas
                                                                                                   // Asegúrate de mapear también las colecciones:
                                                                                                   // .ForMember(dest => dest.AreasDeLaOrden, opt => opt.MapFrom(src => src.AreaOrdenes));
                                                                                                   // ... y así sucesivamente para Piezas, Entregas, InsumosOrden

            //CreateMap<Orden, OrdenDetalleDTO>()
            //    .ForAllMembers(opts => opts
            //    .Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Orden,OrdenDetalleDTO>()
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre))
                .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area.NombreArea));
            //CreateMap<Orden, OrdenDetalleDTO>()
            //    .ForMember(o => o.NombreArea, opcion => opcion
            //    .MapFrom(a => a.Area.NombreArea));


            //CreateMap<Orden, OrdenDetalleDTO>()
            //    .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.Nombre)).ReverseMap();


            #region
            //CreateMap<Entrega, CrearEntregaDTO>();
            //CreateMap<CrearEntregaDTO, Entrega>();

            //// Mapeo inverso de OrdenDetalleDTO a CrearOrdenDTO para el PUT (si lo necesitas)
            //CreateMap<OrdenDetalleDTO, CrearOrdenDTO>();

            ////POST y PUT
            CreateMap<CrearAreaDTO, Area>();

            ////GET y DELETE
            CreateMap<Area, CrearAreaDTO>().ReverseMap();
            //CreateMap<Cliente,CrearClienteDTO>().ReverseMap();


            //// Mapeo para Cliente a ClienteDTO
            //CreateMap<Cliente, ClienteDTO>()
            //    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre)); // O el nombre real de tu propiedad

            //// Mapeo para Area a AreaDTO
            //CreateMap<Area, AreaDTO>()
            //    .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.NombreArea)); // O el nombre real de tu propiedad

            //// Mapeo de CrearOrdenDTO a Entidad Orden
            //CreateMap<CrearOrdenDTO, Orden>()
            //    .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId)); // Este mapeo es CLAVE para AreaId

            //// Asegúrate que tu entidad Orden tenga un "public int AreaId { get; set; }"

            //CreateMap<CrearOrdenDTO, Orden>().ForMember(x=>x.ClienteId,c=>c.MapFrom(a=>a.ClienteId));

            //// ... otros mapeos que ya tenías (CrearClienteDTO <-> Cliente, OrdenDetalleDTO, etc.)



            ////// ... (Otros mapeos existentes, ej. CrearOrdenDTO a Orden, con .ForMember para AreaId) ...
            ////CreateMap<CrearOrdenDTO, Orden>()
            ////    .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId)); // Asegúrate de que este mapeo esté aquí.

            ////// Mapeo para Cliente a ClienteDTO
            ////CreateMap<Cliente, ClienteDTO>()
            ////    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre));

            ////// Mapeo para Area a AreaDTO
            ////CreateMap<Area, AreaDTO>()
            ////    .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.NombreArea));

            ////// Mapeo PRINCIPAL para OrdenDetalleDTO
            ////CreateMap<Orden, OrdenDetalleDTO>()
            ////    // ELIMINAR O COMENTAR estas líneas si existen:
            ////    // .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Cliente.NombreCliente))
            ////    // .ForMember(dest => dest.EmailCliente, opt => opt.MapFrom(src => src.Cliente.EmailCliente))
            ////    // .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area.NombreArea)) // Si existía

            ////    // Asegúrate de mantener los mapeos para colecciones anidadas si los usas
            ////    .ForMember(dest => dest.AreasDeLaOrden, opt => opt.MapFrom(src => src.Area))
            ////    .ForMember(dest => dest.EntregasDeLaOrden, opt => opt.MapFrom(src => src.Entregas));

            ////// ... (Otros mapeos inversos, etc.) ...
            #endregion

            //CreateMap<CrearEntregaDTO, Entrega>().ReverseMap();
            CreateMap<CrearEntregaDTO, Entrega>()
                .ForMember(dest => dest.OrdenId, opt => opt
                .MapFrom(src => src.IdOrden)).ReverseMap();

        }
    }
}