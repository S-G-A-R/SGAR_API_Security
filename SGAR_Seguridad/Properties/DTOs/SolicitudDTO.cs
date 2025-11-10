namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseSolicitud<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }


    public class SolicitudRequest
    {
        public int Id { get; set; }

        public int IdCiudadano { get; set; }

        public int IdOrganizacion { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public int Estado { get; set; }

    }

    public class SolicitudResponse
    {
        public int Id { get; set; }

        public int IdCiudadano { get; set; }

        public int IdOrganizacion { get; set; }

        public DateTime FechaSolicitud { get; set; }

        public int Estado { get; set; } 

    }
}
