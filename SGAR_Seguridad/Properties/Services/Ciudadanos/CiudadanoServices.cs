using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Ciudadanos
{
    public class CiudadanoServices : ICiudadanoServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public CiudadanoServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<CiudadanoResponse> GetCiudadano(int ciudadanoId)
        {
            var ciudadanoIdget = await _db.Ciudadanos.FindAsync(ciudadanoId);
            var ciudadanoResponse = _mapper.Map<Ciudadano, CiudadanoResponse>(ciudadanoIdget);

            return ciudadanoResponse;
        }

        public async Task<int> PostCiudadano(CiudadanoRequest ciudadano)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Ciudadanos.AnyAsync(p =>
                p.IdUser == ciudadano.IdUser);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de ciudadano con el mismos Id.");
            }

            var ciudadanoRequest = _mapper.Map<CiudadanoRequest, Ciudadano>(ciudadano);

            await _db.Ciudadanos.AddAsync(ciudadanoRequest);
            await _db.SaveChangesAsync();
            return ciudadanoRequest.Id;
        }

        public async Task<int> DeleteCiudadano(int ciudadanoId)
        {
            var ciudadano = await _db.Ciudadanos.FindAsync(ciudadanoId);
            if (ciudadano == null)
                return -1;
            _db.Ciudadanos.Remove(ciudadano);
            return await _db.SaveChangesAsync();
        }

        public async Task<CiudadanoResponse> SearchIdUser(int idUser)
        {
            var ciudadano = await _db.Ciudadanos
                .FirstOrDefaultAsync(c => c.IdUser == idUser);
            
            if (ciudadano == null)
                return null;
                
            var ciudadanoResponse = _mapper.Map<Ciudadano, CiudadanoResponse>(ciudadano);
            return ciudadanoResponse;
        }

        public async Task<PaginatedResponseCiudadano<CiudadanoResponse>> GetCiudadanos(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Ciudadanos.AsNoTracking()
                .OrderByDescending(c => c.Id);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<CiudadanoResponse>>(users);

            return new PaginatedResponseCiudadano<CiudadanoResponse>
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
