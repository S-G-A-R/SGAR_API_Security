using SGAR_Seguridad.Properties.Models;
using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Rols
{
    public interface IRoleService
    {
        Task<List<RolResponse>> GetRoles();

    }
}
