using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Solicitudes
{
    public class SolicitudServices : ISolicitudServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public SolicitudServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<SolicitudResponse> GetSolicitud(int solicitudId)
        {
            var solicitudget = await _db.SolicitudesOperadors.FindAsync(solicitudId);
            var solicitudResponse = _mapper.Map<SolicitudesOperador, SolicitudResponse>(solicitudget);

            return solicitudResponse;
        }

        public async Task<int> PostSolicitud(SolicitudRequest solicitud)
        {
            var solicitudRequest = _mapper.Map<SolicitudRequest, SolicitudesOperador>(solicitud);

            await _db.SolicitudesOperadors.AddAsync(solicitudRequest);
            await _db.SaveChangesAsync();
            return solicitudRequest.Id;
        }

        public async Task<int> PutSolicitud(int solicitudId, SolicitudRequest solicitud)
        {
            var entity = await _db.SolicitudesOperadors.FindAsync(solicitudId);
            if (entity == null)
            {
                return -1;
            }
            entity.IdCiudadano = solicitud.IdCiudadano;
            entity.IdOrganizacion = solicitud.IdOrganizacion;
            entity.FechaSolicitud = solicitud.FechaSolicitud;
            entity.Estado = solicitud.Estado;

            // Actualiza la entidad en la base de datos
            _db.SolicitudesOperadors.Update(entity);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteSolicitud(int solicitudId)
        {
            var solicitud = await _db.SolicitudesOperadors.FindAsync(solicitudId);
            if (solicitud == null)
                return -1;
            _db.SolicitudesOperadors.Remove(solicitud);
            return await _db.SaveChangesAsync();
        }

        public async Task<PaginatedResponseSolicitud<SolicitudResponse>> GetSolicitudes(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.SolicitudesOperadors.AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<SolicitudResponse>>(users);

            return new PaginatedResponseSolicitud<SolicitudResponse>
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
