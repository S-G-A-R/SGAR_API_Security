namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponsePuntuacion<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    public class PuntuacionResponse
    {
        public int Id { get; set; }
        public int Puntos { get; set; }
        public string? NombreAnonimo { get; set; }
        public int? IdUser { get; set; }
    }

    public class PuntuacionRequest
    {
        public int Puntos { get; set; }
        public string? NombreAnonimo { get; set; }
        public int? IdUser { get; set; }
    }

    public class PuntuacionRequestConIdUser
    {
        public int Puntos { get; set; }
        public int IdUser { get; set; }
    }
    
    public class PuntuacionRequestSinIdUser
    {
        public int Puntos { get; set; }
        public string? NombreAnonimo { get; set; }
    }
}
