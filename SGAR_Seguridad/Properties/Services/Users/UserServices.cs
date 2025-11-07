using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Models;

namespace SGAR_Seguridad.Properties.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly SgarSecurityDbContext _db;
        private readonly IMapper _mapper;

        public UserServices(SgarSecurityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserResponse> GetUser(int userId)
        {
            var usuario = await _db.Usuarios.FindAsync(userId);
            var userResponse = _mapper.Map<Usuario, UserResponse>(usuario);

            return userResponse;
        }

        public async Task<int> PostUser(UserRequest user)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Usuarios.AnyAsync(p =>
                p.Nombre == user.Nombre &&
                p.Apellido == user.Apellido &&
                p.Dui == user.Dui &&
                p.Email == user.Email &&
                p.Telefono == user.Telefono);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
            }

            var userRequest = _mapper.Map<UserRequest, Usuario>(user);

            // Hashea la contraseña usando BCrypt
            userRequest.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

            await _db.Usuarios.AddAsync(userRequest);
            await _db.SaveChangesAsync();
            return userRequest.Id;
        }

        public async Task<int> PutUser(int userId, UserRequest user)
        {
            var entity = await _db.Usuarios.FindAsync(userId);
            if (entity == null)
            {
                return -1;
            }

            var exists = await _db.Usuarios.AnyAsync(p =>
            p.Nombre == user.Nombre &&
            p.Apellido == user.Apellido &&
            p.Telefono == user.Telefono &&
            p.Email == user.Email);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
            }

            // Almacena la contraseña actual antes del mapeo
            var currentPassword = entity.Password;
            _mapper.Map(user, entity);

            // Verifica si la nueva contraseña no es nula o vacía
            if (!string.IsNullOrEmpty(user.Password))
            {
                // Si hay una nueva contraseña, la hashea y actualiza la entidad
                entity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }
            else
            {
                // Si la contraseña es nula o vacía en la solicitud,
                // restablece la contraseña original para que no se borre
                entity.Password = currentPassword;
            }

            // Actualiza la entidad en la base de datos
            _db.Usuarios.Update(entity);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(int userId)
        {
            var user = await _db.Usuarios.FindAsync(userId);
            if (user == null)
                return -1;
            _db.Usuarios.Remove(user);
            return await _db.SaveChangesAsync();
        }

        public async Task<CredencialesResponse> Login(CredencialesRequest user)
        {
            var userEntity = await _db.Usuarios
                .FirstOrDefaultAsync(o => o.Email == user.Email);

            if (userEntity == null)
            {
                return null; // El email no existe
            }

            // Asegúrate de que los argumentos estén en este orden
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userEntity.Password))
            {
                return null; // La contraseña no coincide
            }

            // Si todo es correcto, el login es exitoso
            var credencialesResponse = new CredencialesResponse
            {
                UserId = userEntity.Id,
                Email = userEntity.Email,
                IdRol = userEntity.IdRol
            };

            return credencialesResponse;
        }

        public async Task<PaginatedResponseUser<UserResponse>> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            var query = _db.Usuarios.AsNoTracking();
            
            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var users = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userResponses = _mapper.Map<List<UserResponse>>(users);

            return new PaginatedResponseUser<UserResponse>
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
