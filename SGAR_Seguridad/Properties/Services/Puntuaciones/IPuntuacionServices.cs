using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Puntuaciones
{
    public interface IPuntuacionServices
    {
        Task<PaginatedResponsePuntuacion<PuntuacionResponse>> GetPuntuaciones(int pageNumber = 1, int pageSize = 10);
        Task<PaginatedResponsePuntuacion<PuntuacionResponse>> SearchPuntuaciones(int? puntos, int pageNumber = 1, int pageSize = 10);
        Task<int> PostPuntuacionConIdUser(PuntuacionRequestConIdUser puntuacion);
        Task<int> PostPuntuacionSinIdUser(PuntuacionRequestSinIdUser puntuacion);
        Task<PuntuacionResponse> GetPuntuacion(int puntuacionId);
        Task<int> PutPuntuacionConIdUser(int puntuacionId, PuntuacionRequestConIdUser puntuacion);
        Task<int> PutPuntuacionSinIdUser(int puntuacionId, PuntuacionRequestSinIdUser puntuacion);
        Task<int> DeletePuntuacion(int puntuacionId);
    }
}
