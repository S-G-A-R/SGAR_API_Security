using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Administradores;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class AdministradorEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/administrador").WithTags("Administraodores");

            //EndPoint para obtener lista los registros de administradores paginados
            group.MapGet("/list", async (int? page, int? pageSize, IAdministradorServices adminService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var admin = await adminService.GetAdministradores(currentPage, size);
                return Results.Ok(admin);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de administrador paginada",
                Description = "Retorna una lista paginada de administrador. Por defecto 10 registros por página.",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para obtener organizacion por id
            group.MapGet("/{id}", async (int id, IAdministradorServices adminService) =>
            {
                var adminget = await adminService.GetAdministrador(id);
                if (adminget == null)
                    return Results.NotFound();
                return Results.Ok(adminget);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener un administrador por ID",
                Description = "Obtiene un administrador específico mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nuevo registro de organizaciones
            group.MapPost("/", async (AdministradorRequest admin, IAdministradorServices adminService) =>
            {
                if (admin == null)
                    return Results.BadRequest();

                var id = await adminService.PostAdministrador(admin);

                return Results.Created($"/api/administrador/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Administrador creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo administrador",
                Description = "Crea un nuevo Administrador en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });
        }
    }
}
