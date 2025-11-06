using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Users;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class OrganizacionEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("/organization").WithTags("Organizaciones");

            //EndPoint para crear nuevo registro de usuarios
            group.MapPost("/", async (UserRequest user, IUserServices userService) =>
            {
                if (user == null)
                    return Results.BadRequest();

                var id = await userService.PostUser(user);

                return Results.Created($"/api/organization/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Organizacion creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nueva Organizacion",
                Description = "Crea un nueva organizacion en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });
        }
    }
}
