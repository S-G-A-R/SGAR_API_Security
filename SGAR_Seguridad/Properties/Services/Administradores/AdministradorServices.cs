using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Administradores
{
    public class AdministradorServices : IAdministradorServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public AdministradorServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<AdministradorResponse> GetAdministrador(int administradorId)
        {
            var adminIdget = await _db.Administradores.FindAsync(administradorId);
            var adminResponse = _mapper.Map<Administradore, AdministradorResponse>(adminIdget);

            return adminResponse;
        }

        public async Task<int> PostAdministrador(AdministradorRequest admin)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Administradores.AnyAsync(p =>
                p.IdUser == admin.IdUser &&
                p.Codigo == admin.Codigo &&
                p.EmailLaboral == admin.EmailLaboral);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de admin con los mismos datos.");
            }

            var adminRequest = _mapper.Map<AdministradorRequest, Administradore>(admin);

            await _db.Administradores.AddAsync(adminRequest);
            await _db.SaveChangesAsync();
            return adminRequest.Id;
        }

        public async Task<int> DeleteAdministrador(int administradorId)
        {
            var admin = await _db.Administradores.FindAsync(administradorId);
            if (admin == null)
                return -1;
            _db.Administradores.Remove(admin);
            return await _db.SaveChangesAsync();
        }

        public async Task<PaginatedResponseAdmin<AdministradorResponse>> GetAdministradores(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Administradores.AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<AdministradorResponse>>(users);

            return new PaginatedResponseAdmin<AdministradorResponse>
            {
                Items = userResponses,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                HasNextPage = pageNumber < totalPages,
                HasPreviousPage = pageNumber > 1
            };
        }

    }
}
