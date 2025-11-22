using AutoMapper;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UserRequest>();
            CreateMap<UserRequest, Usuario>();

            CreateMap<Usuario, UserResponse>();
            CreateMap<CredencialesRequest, Usuario>();


            CreateMap<Organizacion, OrganizationRequest>();
            CreateMap<OrganizationRequest, Organizacion>();

            CreateMap<Organizacion, OrganizationResponse>();
            CreateMap<CredencialesOrganizationRequest, Organizacion>();

            CreateMap<Ciudadano, CiudadanoResponse>();
            CreateMap<CiudadanoRequest, Ciudadano>();

            CreateMap<Operadore, OperadorResponse>();
            CreateMap<OperadorRequest, Operadore>();

            CreateMap<Administradore, AdministradorResponse>();
            CreateMap<AdministradorRequest, Administradore>();

            CreateMap<SolicitudesOperador, SolicitudResponse>();
            CreateMap<SolicitudRequest, SolicitudesOperador>();

            CreateMap<Puntuacion, PuntuacionResponse>();
            CreateMap<PuntuacionRequest, Puntuacion>();

            CreateMap<PuntuacionRequestConIdUser, Puntuacion>();
            CreateMap<PuntuacionRequestSinIdUser, Puntuacion>();

            CreateMap<Role, RolResponse>();
        }
    }
}
