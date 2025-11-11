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

        public async Task<int> PostOperadorWithFile(CreateOperadorWithFileRequest operador, IFormFile? file)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Operadores.AnyAsync(p =>
                p.IdUser == operador.IdUser &&
                p.CodigoOperador == operador.CodigoOperador &&
                p.IdVehiculo == operador.IdVehiculo);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de operador con los mismos datos.");
            }

            var operadorEntity = new Operadore
            {
                IdUser = operador.IdUser,
                CodigoOperador = operador.CodigoOperador,
                IdVehiculo = operador.IdVehiculo,
                IdOrganizacion = operador.IdOrganizacion
            };

            // Manejar el archivo si se proporciona
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Solo se permiten documentos con formato PDF, DOC o DOCX.");
                }

                if (file.Length > 10 * 1024 * 1024)
                {
                    throw new InvalidOperationException("El tamaño del documento no puede exceder 10MB.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    operadorEntity.LicenciaDoc = memoryStream.ToArray();
                }
            }

            await _db.Operadores.AddAsync(operadorEntity);
            await _db.SaveChangesAsync();
            return operadorEntity.Id;
        }

        public async Task<int> DeleteOperador(int operadorId)
        {
            var operador = await _db.Operadores.FindAsync(operadorId);
            if (operador == null)
                return -1;
            _db.Operadores.Remove(operador);
            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutOperador(int operadorId, OperadorRequest operador)
        {
            var entity = await _db.Operadores.FindAsync(operadorId);
            if (entity == null)
            {
                return -1;
            }

            // Verificar si existe otro operador (diferente al actual) con los mismos datos
            var exists = await _db.Operadores.AnyAsync(p =>
                p.Id != operadorId && // Excluir el operador actual
                p.CodigoOperador == operador.CodigoOperador &&
                p.IdVehiculo == operador.IdVehiculo);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de operador con los mismos datos.");
            }

            // Actualizar las propiedades de la entidad con los valores recibidos
            entity.IdUser = operador.IdUser;
            entity.CodigoOperador = operador.CodigoOperador;
            entity.IdVehiculo = operador.IdVehiculo;
            entity.IdOrganizacion = operador.IdOrganizacion;

            // Actualizar el documento de licencia si se proporciona
            if (operador.LicenciaDoc != null)
            {
                entity.LicenciaDoc = operador.LicenciaDoc;
            }

            // Actualiza la entidad en la base de datos
            _db.Operadores.Update(entity);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutOperadorWithFile(int operadorId, UpdateOperadorWithFileRequest operador, IFormFile? file)
        {
            var entity = await _db.Operadores.FindAsync(operadorId);
            if (entity == null)
            {
                return -1;
            }

            // Verificar si existe otro operador (diferente al actual) con los mismos datos
            var exists = await _db.Operadores.AnyAsync(p =>
                p.Id != operadorId &&
                p.CodigoOperador == operador.CodigoOperador &&
                p.IdVehiculo == operador.IdVehiculo);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de operador con los mismos datos.");
            }

            // Actualizar las propiedades básicas
            entity.IdUser = operador.IdUser;
            entity.CodigoOperador = operador.CodigoOperador;
            entity.IdVehiculo = operador.IdVehiculo;
            entity.IdOrganizacion = operador.IdOrganizacion;

            // Manejar el archivo - solo se actualiza si se proporciona uno nuevo
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Solo se permiten documentos con formato PDF, DOC o DOCX.");
                }

                if (file.Length > 10 * 1024 * 1024)
                {
                    throw new InvalidOperationException("El tamaño del documento no puede exceder 10MB.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    entity.LicenciaDoc = memoryStream.ToArray();
                }
            }
            // Si no se envía archivo, se mantiene el documento actual

            _db.Operadores.Update(entity);
            return await _db.SaveChangesAsync();
        }

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
