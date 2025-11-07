using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Users
{
    public interface IUserServices
    {
        Task<PaginatedResponseUser<UserResponse>> GetUsers(int pageNumber = 1, int pageSize = 10);
        Task<int> PostUser(UserRequest user);
        Task<UserResponse> GetUser(int userId);
        Task<int> PutUser(int userId, UserRequest user);
        Task<int> DeleteUser(int userId);
        Task<CredencialesResponse> Login(CredencialesRequest user);
    }
}
