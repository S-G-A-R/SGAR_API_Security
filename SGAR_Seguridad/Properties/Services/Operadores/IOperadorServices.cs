using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Operadores
{
    public interface IOperadorServices
    {
        Task<PaginatedResponseOperador<OperadorResponse>> GetOperadores(int pageNumber = 1, int pageSize = 10);
        Task<PaginatedResponseOperador<OperadorResponse>> SearchOperadores(string? codigoOperador, int? idUser, int? idOrganizacion, int pageNumber = 1, int pageSize = 10);
        Task<int> PostOperador(OperadorRequest operador);
        Task<int> PostOperadorWithFile(CreateOperadorWithFileRequest operador, IFormFile? file);
        Task<int> PutOperador(int operadorId, OperadorRequest operador);
        Task<int> PutOperadorWithFile(int operadorId, UpdateOperadorWithFileRequest operador, IFormFile? file);
        Task<int> DeleteOperador(int operadorId);
        Task<OperadorResponse> GetOperador(int operadorId);
    }
}

