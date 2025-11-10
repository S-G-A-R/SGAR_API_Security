using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Solicitudes
{
    public interface ISolicitudServices
    {
        Task<PaginatedResponseSolicitud<SolicitudResponse>> GetSolicitudes(int pageNumber = 1, int pageSize = 10);
        Task<int> PostSolicitud(SolicitudRequest solicitud);
        Task<SolicitudResponse> GetSolicitud(int solicitudId);
        Task<int> PutSolicitud(int solicitudId, SolicitudRequest solicitud);
        Task<int> DeleteSolicitud(int solicitudId);

    }
}
