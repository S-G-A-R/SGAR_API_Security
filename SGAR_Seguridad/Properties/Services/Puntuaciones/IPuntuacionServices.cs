using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Puntuaciones
{
    public interface IPuntuacionServices
    {
        Task<PaginatedResponsePuntuacion<PuntuacionResponse>> GetPuntuaciones(int pageNumber = 1, int pageSize = 10);
        Task<int> PostPuntuacion(PuntuacionRequest puntuacion);
        Task<PuntuacionResponse> GetPuntuacion(int puntuacionId);
        Task<int> PutPuntuacion(int puntuacionId, PuntuacionRequest puntuacion);
        Task<int> DeletePuntuacion(int puntuacionId);
    }
}
