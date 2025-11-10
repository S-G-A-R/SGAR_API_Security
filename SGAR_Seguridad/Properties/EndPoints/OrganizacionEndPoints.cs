using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Operadores;
using SGAR_Seguridad.Properties.Services.Organizations;
using SGAR_Seguridad.Properties.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class OrganizacionEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/organization").WithTags("Organizaciones");

            //EndPoint para obtener lista los registros de usuario
            group.MapGet("/list", async (int? page, int? pageSize, IOrganizacionServices orgService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var users = await orgService.GetOrganizations(currentPage, size);
                return Results.Ok(users);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de organizacion paginada",
                Description = "Retorna una lista paginada de usuarios. Por defecto 10 registros por página.",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para obtener organizacion por id
            group.MapGet("/{id}", async (int id, IOrganizacionServices orgService) =>
            {
                var getorg = await orgService.GetOrganization(id);
                if (getorg == null)
                    return Results.NotFound();
                return Results.Ok(getorg);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener una organizacion por ID",
                Description = "Obtiene una organizacion específico mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nuevo registro de organizaciones
            group.MapPost("/", async (OrganizationRequest orgUser, IOrganizacionServices organizacionService) =>
            {
                if (orgUser == null)
                    return Results.BadRequest();

                var id = await organizacionService.PostOrganization(orgUser);

                return Results.Created($"/api/post/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Organizacion creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo organizacion",
                Description = "Crea un nueva organizacion en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // EndPoint para eliminar un registro de organizacion
            group.MapDelete("/{id}", async (int id, IOrganizacionServices orgService) =>
            {
                var result = await orgService.DeleteOrganization(id);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró la organizacion con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {
                        // Mensaje de éxito explícito
                        message = "¡Organizacion eliminado exitosamente!",
                        id = id
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar organizacion",
                Description = "Elimina una organizacion existente mediante su ID",
            });

            //EndPoint para actualizar un registro de organizacion
            group.MapPut("/{id}", async (int id, OrganizationRequest orgUpdate, IOrganizacionServices orgService) =>
            {
                var result = await orgService.PutOrganization(id, orgUpdate);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró el organizacion con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {  // Mensaje de éxito explícito
                        message = "¡Organizacion actualizado exitosamente!",
                        Id = id,
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar organizacion",
                Description = "Actualiza la información de un organizacion existente",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para generar token 
            group.MapPost("/login", async (CredencialesOrganizationRequest orgUser, IOrganizacionServices orgService, IConfiguration config) =>
            {
                var login = await orgService.Login(orgUser);
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
                        5 => "Organizacion"
                        //_ => "Organizacion"
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(new[]
                         {
                            new Claim(ClaimTypes.NameIdentifier, login.orgUserId.ToString()),
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
                Summary = "Login Organizacion",
                Description = "Generar toke para autenticacion",
            });

        }

    }
    
}
