namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseOrganization<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
    public class OrganizationResponse
    {
        public int Id { get; set; }
        public string IdMunicipio { get; set; } = null!;
        public string NombreOrganizacion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int IdRol { get; set; }
    }
    public class OrganizationRequest
    {
        public string IdMunicipio { get; set; } = null!;
        public string NombreOrganizacion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRol { get; set; }
    }
}
