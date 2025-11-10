namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseAdmin<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class AdministradorResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string EmailLaboral { get; set; } = null!;
        public string TelefonoLaboral { get; set; } = null!;
        public int IdUser { get; set; }
    }

    public class AdministradorRequest
    {
        public string Codigo { get; set; } = null!;
        public string EmailLaboral { get; set; } = null!;
        public string TelefonoLaboral { get; set; } = null!;
        public int IdUser { get; set; }
    }
}
