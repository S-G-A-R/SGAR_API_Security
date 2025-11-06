namespace SGAR_Seguridad.Properties.DTOs
{
    public class OrganizationResponse
    {
        public int Id { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreOrganizacion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Email { get; set; }
        public string Password { get; set; } = null!;
    }
    public class OrganizationRequest
    {
        public int Id { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreOrganizacion { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Email { get; set; }
    }
}
