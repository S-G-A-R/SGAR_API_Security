using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Organizations
{
    public class OrganizacionServices : IOrganizacionServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public OrganizacionServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OrganizationResponse> GetOrganization(int organizationId)
        {
            var organizationget = await _db.Organizacions.FindAsync(organizationId);
            var organizacionResponse = _mapper.Map<Organizacion, OrganizationResponse>(organizationget);

            return organizacionResponse;
        }

        public async Task<int> PostOrganization(OrganizationRequest org)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Organizacions.AnyAsync(p =>
                p.NombreOrganizacion == org.NombreOrganizacion &&
                p.Telefono == org.Telefono &&
                p.Email == org.Email);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de organizacion con los mismos datos.");
            }

            var orgRequest = _mapper.Map<OrganizationRequest, Organizacion>(org);

            // Hashea la contraseña usando BCrypt
            orgRequest.Password = BCrypt.Net.BCrypt.HashPassword(orgRequest.Password);

            await _db.Organizacions.AddAsync(orgRequest);
            await _db.SaveChangesAsync();
            return orgRequest.Id;
        }

        public async Task<int> DeleteOrganization(int orgId)
        {
            var org = await _db.Organizacions.FindAsync(orgId);
            if (org == null)
                return -1;
            _db.Organizacions.Remove(org);
            return await _db.SaveChangesAsync();
        }

        public async Task<CredencialesOrganizationResponse> Login(CredencialesOrganizationRequest orgUser)
        {
            var userEntity = await _db.Organizacions
                .FirstOrDefaultAsync(o => o.Email == orgUser.Email);

            if (userEntity == null)
            {
                return null; // El email no existe
            }

            // Asegúrate de que los argumentos estén en este orden
            if (!BCrypt.Net.BCrypt.Verify(orgUser.Password, userEntity.Password))
            {
                return null; // La contraseña no coincide
            }

            // Si todo es correcto, el login es exitoso
            var CredencialesOrganizationResponse = new CredencialesOrganizationResponse
            {
                orgUserId = userEntity.Id,
                Email = userEntity.Email,
                IdRol = userEntity.IdRol
            };

            return CredencialesOrganizationResponse;
        }

        public async Task<PaginatedResponseOrganization<OrganizationResponse>> GetOrganizations(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Organizacions.AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<OrganizationResponse>>(users);

            return new PaginatedResponseOrganization<OrganizationResponse>
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
