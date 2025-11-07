using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Ciudadanos;
using SGAR_Seguridad.Properties.Services.Organizations;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class CiudadanoEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/ciudadano").WithTags("Ciudadanos");

            //EndPoint para obtener lista los registros de ciudadanos paginados
            group.MapGet("/list", async (int? page, int? pageSize, ICiudadanoServices ciudadanoService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var ciudadano = await ciudadanoService.GetCiudadanos(currentPage, size);
                return Results.Ok(ciudadano);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de ciudadano paginada",
                Description = "Retorna una lista paginada de ciudadno. Por defecto 10 registros por página.",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para obtener organizacion por id
            group.MapGet("/{id}", async (int id, ICiudadanoServices ciudadanoService) =>
            {
                var getorg = await ciudadanoService.GetCiudadano(id);
                if (getorg == null)
                    return Results.NotFound();
                return Results.Ok(getorg);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener un ciudadano por ID",
                Description = "Obtiene un ciudadano específico mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nuevo registro de organizaciones
            group.MapPost("/", async (CiudadanoRequest ciudadano, ICiudadanoServices ciudadanoService) =>
            {
                if (ciudadano == null)
                    return Results.BadRequest();

                var id = await ciudadanoService.PostCiudadano(ciudadano);

                return Results.Created($"/api/ciudadano/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Ciudadano creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo ciudadano",
                Description = "Crea un nuevo ciudadano en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // EndPoint para eliminar un registro de ciudadano
            group.MapDelete("/{id}", async (int id, ICiudadanoServices ciudadanoService) =>
            {
                var result = await ciudadanoService.DeleteCiudadano(id);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró la ciudadano con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {
                        // Mensaje de éxito explícito
                        message = "¡Ciudadano eliminado exitosamente!",
                        id = id
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar ciudadano",
                Description = "Elimina una ciudadano existente mediante su ID",
            });
        }
    }

}
