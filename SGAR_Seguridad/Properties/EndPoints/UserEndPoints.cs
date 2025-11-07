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
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


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
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

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
                Summary = "Crear nuevo usuario",
                Description = "Crea un nuevo usuario en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para actualizar un registro de personal
            group.MapPut("/{id}", async (int id, UserRequest userUpdate, IUserServices userService) =>
            {
                var result = await userService.PutUser(id, userUpdate);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró el usuario con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {  // Mensaje de éxito explícito
                        message = "¡Usuario actualizado exitosamente!",
                        Id = id,
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar usuario",
                Description = "Actualiza la información de un usuario existente",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

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
            });

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
                Description = "Generar toke para autenticacion",
            });

        }
    }
}
