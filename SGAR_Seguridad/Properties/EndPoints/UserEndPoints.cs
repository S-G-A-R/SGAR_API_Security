using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class UserEndPoints
    {
        public static void add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/user").WithTags("users");

            //EndPoint para obtener lista los registros de usuario
            group.MapGet("/list", async (int? page, int? pageSize, IUserServices userService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;
                
                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página
                
                var users = await userService.GetUsers(currentPage, size);
                return Results.Ok(users);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de usuarios paginada",
                Description = "Retorna una lista paginada de usuarios. Por defecto 10 registros por página.",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });


            //EndPoint para buscar usuarios por criterios
            group.MapGet("/search", async (
                string? nombre, 
                string? apellido, 
                string? telefono, 
                string? email, 
                int? idRol,
                int? page, 
                int? pageSize, 
                IUserServices userService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;
                
                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página
                
                var users = await userService.SearchUsers(nombre, apellido, telefono, email, idRol, currentPage, size);
                return Results.Ok(users);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Buscar usuarios por criterios",
                Description = "Busca usuarios filtrando por nombre, apellido, teléfono, email y/o rol. Todos los parámetros son opcionales. Retorna resultados paginados.",
            }).RequireAuthorization(policy => policy.RequireRole("Administrador", "Operador", "Ciudadano", "Asociado", "Organizacion"));

            //EndPoint para obtener usuario por id
            group.MapGet("/{id}", async (int id, IUserServices userService) =>
            {
                var getusuario = await userService.GetUser(id);
                if (getusuario == null)
                    return Results.NotFound();
                return Results.Ok(getusuario);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener un usuario por ID",
                Description = "Obtiene un usuario específico mediante su ID",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            //EndPoint para crear nuevo registro de usuarios
            group.MapPost("/", async (UserRequest user, IUserServices userService) =>
            {
                if (user == null)
                    return Results.BadRequest();

                var id = await userService.PostUser(user);

                return Results.Created($"/api/user/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Usuario creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo usuario (JSON)",
                Description = "Crea un nuevo usuario en el sistema. Puede incluir una foto en formato base64 en el campo FotoBase64",
            });

            //EndPoint para crear nuevo registro de usuarios con archivo de imagen
            group.MapPost("/create", async (
                [Microsoft.AspNetCore.Mvc.FromForm] string nombre,
                [Microsoft.AspNetCore.Mvc.FromForm] string apellido,
                [Microsoft.AspNetCore.Mvc.FromForm] string email,
                [Microsoft.AspNetCore.Mvc.FromForm] string dui,
                [Microsoft.AspNetCore.Mvc.FromForm] string password,
                [Microsoft.AspNetCore.Mvc.FromForm] int idRol,
                [Microsoft.AspNetCore.Mvc.FromForm] string? telefono,
                IFormFile? file,
                IUserServices userService) =>
            {
                try
                {
                    var userRequest = new CreateUserWithFileRequest
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Dui = dui,
                        Password = password,
                        IdRol = idRol,
                        Telefono = telefono
                    };

                    var id = await userService.PostUserWithFile(userRequest, file);

                    return Results.Created($"/api/user/{id}", new
                    {
                        message = "¡Usuario creado exitosamente con foto!",
                        Id = id
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo usuario con foto (Form-Data)",
                Description = "Crea un nuevo usuario en el sistema cargando directamente una imagen. La foto es opcional. Formatos permitidos: JPG, JPEG, PNG, GIF, BMP. Tamaño máximo: 5MB",
            }).DisableAntiforgery();

            //EndPoint para actualizar un registro de usuario (JSON)
            group.MapPut("/{id}", async (int id, UserRequest userUpdate, IUserServices userService) =>
            {
                try
                {
                    var result = await userService.PutUser(id, userUpdate);
                    if (result == -1)
                        return Results.NotFound(new
                        {
                            message = "No se encontró el usuario con el ID proporcionado."
                        });
                    else
                        return Results.Ok(new
                        {
                            message = "¡Usuario actualizado exitosamente!",
                            Id = id,
                        });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar usuario (JSON)",
                Description = "Actualiza la información de un usuario existente. Puede incluir una foto en formato base64 en el campo FotoBase64",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            //EndPoint para actualizar usuario con archivo de imagen
            group.MapPut("/update/{id}", async (
                int id,
                [Microsoft.AspNetCore.Mvc.FromForm] string nombre,
                [Microsoft.AspNetCore.Mvc.FromForm] string apellido,
                [Microsoft.AspNetCore.Mvc.FromForm] string email,
                [Microsoft.AspNetCore.Mvc.FromForm] string dui,
                [Microsoft.AspNetCore.Mvc.FromForm] int idRol,
                [Microsoft.AspNetCore.Mvc.FromForm] string? telefono,
                [Microsoft.AspNetCore.Mvc.FromForm] string? password,
                IFormFile? file,
                IUserServices userService) =>
            {
                try
                {
                    var userRequest = new UpdateUserWithFileRequest
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Dui = dui,
                        IdRol = idRol,
                        Telefono = telefono,
                        Password = password
                    };

                    var result = await userService.PutUserWithFile(id, userRequest, file);
                    if (result == -1)
                        return Results.NotFound(new
                        {
                            message = "No se encontró el usuario con el ID proporcionado."
                        });
                    else
                        return Results.Ok(new
                        {
                            message = "¡Usuario actualizado exitosamente!",
                            Id = id
                        });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar usuario con foto (Form-Data)",
                Description = "Actualiza un usuario existente con opción de actualizar o mantener la foto. " +
                             "- Si envías un archivo: Se actualiza la foto\n" +
                             "- Si NO envías archivo: Se mantiene la foto actual\n" +
                             "- El campo 'password' es opcional, déjalo vacío para no cambiarlo\n" +
                             "Formatos permitidos: JPG, JPEG, PNG, GIF, BMP. Tamaño máximo: 5MB",
            }).DisableAntiforgery().RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            // EndPoint para eliminar un registro de usuario
            group.MapDelete("/{id}", async (int id, IUserServices userService) =>
            {
                var result = await userService.DeleteUser(id);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró el usuario con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {
                        // Mensaje de éxito explícito
                        message = "¡Usuario eliminado exitosamente!",
                        id = id
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar usuario",
                Description = "Elimina un usuario existente mediante su ID",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Organizacion" });

            //EndPoint para actualizar el rol de un usuario
            group.MapPut("/{id}/role", async (int id, UpdateRolRequest userUpdateRol, IUserServices userService) =>
            {
                try
                {
                    var result = await userService.PutRolUser(id, userUpdateRol);
                    if (result == -1)
                        return Results.NotFound(new
                        {
                            message = "No se encontró el usuario con el ID proporcionado."
                        });
                    else
                        return Results.Ok(new
                        {
                            message = "¡Rol de usuario actualizado exitosamente!",
                            Id = id,
                        });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar rol de usuario",
                Description = "Actualiza únicamente el rol de un usuario existente.",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Organizacion" });

            //EndPoint para generar token 
            group.MapPost("/login", async (CredencialesRequest user, IUserServices userService, IConfiguration config) =>
            {
                var login = await userService.Login(user);
                if (login == null)
                    return Results.Unauthorized();
                else
                {
                    var JwtSetting = config.GetSection("JwtSetting");
                    var secretKey = JwtSetting.GetValue<string>("SecretKey");
                    var issuer = JwtSetting.GetValue<string>("Issuer");
                    var audience = JwtSetting.GetValue<string>("Audience");

                    var tokenHadler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretKey);

                    var roleName = login.IdRol switch
                    {
                        1 => "Ciudadano",
                        2 => "Operador",
                        3 => "Asociado",
                        4 => "Administrador",
                        //_ => "Organizacion"
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(new[]
                         {
                            new Claim(ClaimTypes.NameIdentifier, login.UserId.ToString()),
                            new Claim(ClaimTypes.Email, login.Email),
                            new Claim(ClaimTypes.Role, roleName),
                         }),
                        Expires = DateTime.UtcNow.AddHours(8),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHadler.CreateToken(tokenDescriptor);

                    var jwt = tokenHadler.WriteToken(token);

                    return Results.Ok(jwt);

                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Login personal",
                Description = "Generar token para autenticacion",
            });

        }
    }
}
