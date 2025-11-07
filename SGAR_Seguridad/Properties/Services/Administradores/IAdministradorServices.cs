using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Administradores
{
    public interface IAdministradorServices
    {
        Task<PaginatedResponseAdmin<AdministradorResponse>> GetAdministradores(int pageNumber = 1, int pageSize = 10);
        Task<int> PostAdministrador(AdministradorRequest administrador);
        Task<int> DeleteAdministrador(int administradorId);
        Task<AdministradorResponse> GetAdministrador(int administradorId);
    }
}
