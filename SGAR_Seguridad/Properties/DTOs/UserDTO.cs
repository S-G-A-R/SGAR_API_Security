namespace SGAR_Seguridad.Properties.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string Email { get; set; } = null!;

        public string Dui { get; set; } = null!;

        public byte[]? Foto { get; set; }

        public int IdRol { get; set; }
    }

    public class UserRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public string Email { get; set; } = null!;

        public string Dui { get; set; } = null!;

        public byte[]? Foto { get; set; }

        public string Password { get; set; } = null!;

        public int IdRol { get; set; }
    }
}
