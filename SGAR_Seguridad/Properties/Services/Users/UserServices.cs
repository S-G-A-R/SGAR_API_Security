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

            // Convertir imagen base64 a byte[] si existe
            if (!string.IsNullOrEmpty(user.FotoBase64))
            {
                try
                {
                    userRequest.Foto = Convert.FromBase64String(user.FotoBase64);
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException("El formato de la imagen base64 no es válido.");
                }
            }

            await _db.Usuarios.AddAsync(userRequest);
            await _db.SaveChangesAsync();
            return userRequest.Id;
        }

        public async Task<int> PostUserWithFile(CreateUserWithFileRequest user, IFormFile? file)
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
                throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
            }

            var usuario = new Usuario
            {
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Telefono = user.Telefono,
                Email = user.Email,
                Dui = user.Dui,
                IdRol = user.IdRol,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            // Manejar el archivo de imagen si se proporciona
            if (file != null && file.Length > 0)
            {
                // Validar el tipo de archivo
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Solo se permiten imágenes con formato JPG, JPEG, PNG, GIF o BMP.");
                }

                // Validar el tamaño del archivo (máximo 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    throw new InvalidOperationException("El tamaño de la imagen no puede exceder 5MB.");
                }

                // Convertir el archivo a byte[]
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    usuario.Foto = memoryStream.ToArray();
                }
            }

            await _db.Usuarios.AddAsync(usuario);
            await _db.SaveChangesAsync();
            return usuario.Id;
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
            p.Email == user.Email &&
            p.Id != userId); // Excluir el usuario actual de la búsqueda

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
            }

            // Almacena la contraseña y foto actuales antes del mapeo
            var currentPassword = entity.Password;
            var currentFoto = entity.Foto;
            
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

            // Manejar la actualización de la foto
            if (!string.IsNullOrEmpty(user.FotoBase64))
            {
                try
                {
                    entity.Foto = Convert.FromBase64String(user.FotoBase64);
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException("El formato de la imagen base64 no es válido.");
                }
            }
            else if (user.Foto == null && currentFoto != null)
            {
                // Si no se envió foto nueva, mantener la actual
                entity.Foto = currentFoto;
            }

            // Actualiza la entidad en la base de datos
            _db.Usuarios.Update(entity);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> PutUserWithFile(int userId, UpdateUserWithFileRequest user, IFormFile? file)
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
                p.Email == user.Email &&
                p.Id != userId);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de usuario con los mismos datos.");
            }

            // Actualizar datos básicos
            entity.Nombre = user.Nombre;
            entity.Apellido = user.Apellido;
            entity.Telefono = user.Telefono;
            entity.Email = user.Email;
            entity.Dui = user.Dui;
            entity.IdRol = user.IdRol;

            // Manejar la contraseña
            if (!string.IsNullOrEmpty(user.Password))
            {
                entity.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            // Manejar la foto - solo se actualiza si se proporciona un nuevo archivo
            if (file != null && file.Length > 0)
            {
                // Si se proporciona un nuevo archivo de foto
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidOperationException("Solo se permiten imágenes con formato JPG, JPEG, PNG, GIF o BMP.");
                }

                if (file.Length > 5 * 1024 * 1024)
                {
                    throw new InvalidOperationException("El tamaño de la imagen no puede exceder 5MB.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    entity.Foto = memoryStream.ToArray();
                }
            }
            // Si no se envía archivo, se mantiene la foto actual (no se elimina)

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

        public async Task<int> PutRolUser(int userId, UpdateRolRequest update)
        {
            var entity = await _db.Usuarios.FindAsync(userId);
            if (entity == null)
            {
                return -1;
            }

            // Actualizar el rol del usuario
            entity.IdRol = update.IdRol;

            _db.Usuarios.Update(entity);
            return await _db.SaveChangesAsync();
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
