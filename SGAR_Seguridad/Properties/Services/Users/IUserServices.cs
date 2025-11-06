using AutoMapper;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Users;

namespace SGAR_Seguridad.Properties.Services.Users
{
    public interface IUserServices
    {
        Task<int> PostUser(UserRequest user);
        //Task<List<UserResponse>> GetUsers();
        Task<UserResponse> GetUser(int userId);
        //Task<int> PutUser(int userlId, UserRequest user);
        //Task<int> DeleteUser(int userId);
        Task<CredencialesResponse> Login(CredencialesRequest user);
        //Task<UserResponse> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo);
    }
}
