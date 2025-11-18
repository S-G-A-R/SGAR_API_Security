using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Ciudadanos
{
    public interface ICiudadanoServices
    {
        Task<PaginatedResponseCiudadano<CiudadanoResponse>> GetCiudadanos(int pageNumber = 1, int pageSize = 10);
        Task<int> PostCiudadano(CiudadanoRequest ciudadano);
        Task<CiudadanoResponse> GetCiudadano(int ciudadanoId);
        Task<int> DeleteCiudadano(int ciudadanoId);
        Task<CiudadanoResponse> SearchIdUser (int idUser);
        //Task<UserResponse> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo);
    }
}
