using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Organizations
{
    public interface IOrganizacionServices
    {
        Task<PaginatedResponseOrganization<OrganizationResponse>> GetOrganizations(int pageNumber = 1, int pageSize = 10);
        Task<int> PostOrganization(OrganizationRequest organization);
        Task<OrganizationResponse> GetOrganization(int organizationId);
        Task<int> PutOrganization(int organizationId, OrganizationRequest organization);
        Task<int> DeleteOrganization(int organizationId);
        Task<CredencialesOrganizationResponse> Login(CredencialesOrganizationRequest orgUser);
        //Task<UserResponse> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo);
    }
}
