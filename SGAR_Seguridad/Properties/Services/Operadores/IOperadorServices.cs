using SGAR_Seguridad.Properties.DTOs;

namespace SGAR_Seguridad.Properties.Services.Operadores
{
    public interface IOperadorServices
    {
        Task<PaginatedResponseOperador<OperadorResponse>> GetOperadores(int pageNumber = 1, int pageSize = 10);
        Task<int> PostOperador(OperadorRequest operador);
        Task<int> DeleteOperador(int operadorId);
        Task<OperadorResponse> GetOperador(int operadorId);
    }
}
