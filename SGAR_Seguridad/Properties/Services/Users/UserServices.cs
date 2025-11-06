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

    }
}
