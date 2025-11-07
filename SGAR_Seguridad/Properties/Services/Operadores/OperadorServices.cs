using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Operadores
{
    public class OperadorServices : IOperadorServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public OperadorServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OperadorResponse> GetOperador(int operadorId)
        {
            var operadorIdget = await _db.Operadores.FindAsync(operadorId);
            var operadorResponse = _mapper.Map<Operadore, OperadorResponse>(operadorIdget);

            return operadorResponse;
        }

        public async Task<int> PostOperador(OperadorRequest operador)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Operadores.AnyAsync(p =>
                p.IdUser == operador.IdUser &&
                p.CodigoOperador == operador.CodigoOperador &&
                p.IdVehiculo == operador.IdVehiculo);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de operador con los mismos datos.");
            }

            var operadorRequest = _mapper.Map<OperadorRequest, Operadore>(operador);

            await _db.Operadores.AddAsync(operadorRequest);
            await _db.SaveChangesAsync();
            return operadorRequest.Id;
        }
        public async Task<int> DeleteOperador(int operadorId)
        {
            var operador = await _db.Operadores.FindAsync(operadorId);
            if (operador == null)
                return -1;
            _db.Operadores.Remove(operador);
            return await _db.SaveChangesAsync();
        }

        //public async Task<int> PutUser(int userId, OperadorRequest operador)
        //{
        //    var entity = await _db.Operadores.FindAsync(userId);
        //    if (entity == null)
        //    {
        //        return -1;
        //    }

        //    var exists = await _db.Operadores.AnyAsync(p =>
        //    p.CodigoOperador == operador.CodigoOperador &&
        //    p.Apellido == user.Apellido &&
        //    p.Telefono == user.Telefono &&
        //    p.Email == user.Email);

        //    if (exists)
        //    {
        //        throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
        //    }

        //    // Actualiza la entidad en la base de datos
        //    _db.Usuarios.Update(entity);

        //    return await _db.SaveChangesAsync();
        //}

        public async Task<PaginatedResponseOperador<OperadorResponse>> GetOperadores(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Operadores.AsNoTracking();

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<OperadorResponse>>(users);

            return new PaginatedResponseOperador<OperadorResponse>
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
