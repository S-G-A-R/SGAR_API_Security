using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.Models;
using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Rols
{
    public class RoleService : IRoleService
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public RoleService(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<RolResponse>> GetRoles()
        {
            var rol = await _db.Roles.ToListAsync();
            var rolList = _mapper.Map<List<Role>, List<RolResponse>>(rol);

            return rolList;

        }
    }
}
