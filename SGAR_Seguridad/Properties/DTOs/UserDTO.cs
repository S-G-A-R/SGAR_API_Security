namespace SGAR_Seguridad.Properties.DTOs
{
    public class PaginatedResponseUser<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

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

        // Propiedad para recibir la foto en formato base64
        public string? FotoBase64 { get; set; }

        public string Password { get; set; } = null!;

        public int IdRol { get; set; }
    }

    // DTO específico para crear usuario con archivo desde Swagger
    public class CreateUserWithFileRequest
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Email { get; set; } = null!;
        public string Dui { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRol { get; set; }
    }

    // DTO específico para actualizar usuario con archivo desde Swagger
    public class UpdateUserWithFileRequest
    {
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Email { get; set; } = null!;
        public string Dui { get; set; } = null!;
        public string? Password { get; set; }
        public int IdRol { get; set; }
    }

    public class UpdateRolRequest
    {
        public int IdRol { get; set; }
    }

}
