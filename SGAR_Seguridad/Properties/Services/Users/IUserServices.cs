using SGAR_Seguridad.Properties.DTOs;
using Microsoft.AspNetCore.Http;

namespace SGAR_Seguridad.Properties.Services.Users
{
    public interface IUserServices
    {
        Task<PaginatedResponseUser<UserResponse>> GetUsers(int pageNumber = 1, int pageSize = 10);
        Task<PaginatedResponseUser<UserResponse>> SearchUsers(string? nombre, string? apellido, string? telefono, string? email, int? idRol, int pageNumber = 1, int pageSize = 10);
        Task<int> PostUser(UserRequest user);
        Task<int> PostUserWithFile(CreateUserWithFileRequest user, IFormFile? file);
        Task<UserResponse> GetUser(int userId);
        Task<int> PutUser(int userId, UserRequest user);
        Task<int> PutUserWithFile(int userId, UpdateUserWithFileRequest user, IFormFile? file);
        Task<int> DeleteUser(int userId);
        Task<CredencialesResponse> Login(CredencialesRequest user);
        Task<int> PutRolUser(int userId, UpdateRolRequest updateRol);
    }
}
