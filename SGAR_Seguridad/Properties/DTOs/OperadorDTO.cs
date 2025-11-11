namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseOperador<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class OperadorResponse
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string CodigoOperador { get; set; } = null!;
        public int IdVehiculo { get; set; }
        public byte[]? LicenciaDoc { get; set; }
        public int IdOrganizacion { get; set; }
    }

    public class OperadorRequest
    {
        public int IdUser { get; set; }
        public string CodigoOperador { get; set; } = null!;
        public int IdVehiculo { get; set; }
        public byte[]? LicenciaDoc { get; set; }
        public int IdOrganizacion { get; set; }
    }

    public class CreateOperadorWithFileRequest
    {
        public int IdUser { get; set; }
        public string CodigoOperador { get; set; } = null!;
        public int IdVehiculo { get; set; }
        public int IdOrganizacion { get; set; }
    }

    public class UpdateOperadorWithFileRequest
    {
        public int IdUser { get; set; }
        public string CodigoOperador { get; set; } = null!;
        public int IdVehiculo { get; set; }
        public int IdOrganizacion { get; set; }
    }
}
