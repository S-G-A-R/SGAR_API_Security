using AutoMapper;
using SGAR_Seguridad.Properties.Models;
using SGAR_Seguridad.Properties.DTOs;

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

        }

    }
}
