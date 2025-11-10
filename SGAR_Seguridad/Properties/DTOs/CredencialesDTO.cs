namespace SGAR_Seguridad.Properties.DTOs
{
    public class CredencialesResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRol { get; set; }
    }

    public class CredencialesRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class  CredencialesOrganizationResponse
    {
        public int orgUserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRol { get; set; }
    }

    public class CredencialesOrganizationRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
