using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Puntuaciones
{
    public class PuntuacionServices : IPuntuacionServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public PuntuacionServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PuntuacionResponse> GetPuntuacion(int puntuacionId)
        {
            var puntuacion = await _db.Puntuacions.FindAsync(puntuacionId);
            var puntuacionResponse = _mapper.Map<Puntuacion, PuntuacionResponse>(puntuacion);

            return puntuacionResponse;
        }

        public async Task<int> PostPuntuacion(PuntuacionRequest puntuacion)
        {
            var puntuacionRequest = _mapper.Map<PuntuacionRequest, Puntuacion>(puntuacion);

            await _db.Puntuacions.AddAsync(puntuacionRequest);
            await _db.SaveChangesAsync();
            return puntuacionRequest.Id;
        }

        public async Task<int> PutPuntuacion(int puntuacionId, PuntuacionRequest puntuacion)
        {
            var puntuacionToUpdate = await _db.Puntuacions.FindAsync(puntuacionId);
            if (puntuacionToUpdate == null)
                return -1;

            _mapper.Map(puntuacion, puntuacionToUpdate);
            
            _db.Puntuacions.Update(puntuacionToUpdate);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeletePuntuacion(int puntuacionId)
        {
            var puntuacion = await _db.Puntuacions.FindAsync(puntuacionId);
            if (puntuacion == null)
                return -1;
            _db.Puntuacions.Remove(puntuacion);
            return await _db.SaveChangesAsync();
        }

        public async Task<PaginatedResponsePuntuacion<PuntuacionResponse>> GetPuntuaciones(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Puntuacions.AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var puntuaciones = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var puntuacionResponses = _mapper.Map<List<PuntuacionResponse>>(puntuaciones);

            return new PaginatedResponsePuntuacion<PuntuacionResponse>
            {
                Items = puntuacionResponses,
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
