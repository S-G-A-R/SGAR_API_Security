namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseCiudadano<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class CiudadanoResponse
    {
        public int Id { get; set; }
        public string IdZona { get; set; } = null!;
        public int IdUser { get; set; }
    }

    public class CiudadanoRequest
    {
        public string IdZona { get; set; } = null!;
        public int IdUser { get; set; }

    }
}
