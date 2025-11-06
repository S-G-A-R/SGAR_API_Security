using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Organizacion
{
    public interface IOrganizacionServices
    {
        Task<int> PostOrganization(UserRequest organization);
        //Task<List<UserResponse>> GetUsers();
        Task<UserResponse> GetOrganization(int organizationId);
        //Task<int> PutUser(int userlId, UserRequest user);
        //Task<int> DeleteUser(int userId);
        Task<CredencialesResponse> Login(CredencialesRequest user);
        //Task<UserResponse> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo);
    }
}
