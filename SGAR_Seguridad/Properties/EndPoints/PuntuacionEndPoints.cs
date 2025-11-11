using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Puntuaciones;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class PuntuacionEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/puntuacion").WithTags("Puntuaciones");

            //EndPoint para obtener lista de puntuaciones paginados
            group.MapGet("/list", async (int? page, int? pageSize, IPuntuacionServices puntuacionService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var puntuaciones = await puntuacionService.GetPuntuaciones(currentPage, size);
                return Results.Ok(puntuaciones);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de puntuaciones paginada",
                Description = "Retorna una lista paginada de puntuaciones. Por defecto 10 registros por página.",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para obtener una puntuación por id
            group.MapGet("/{id}", async (int id, IPuntuacionServices puntuacionService) =>
            {
                var puntuacion = await puntuacionService.GetPuntuacion(id);
                if (puntuacion == null)
                    return Results.NotFound();
                return Results.Ok(puntuacion);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener una puntuación por ID",
                Description = "Obtiene una puntuación específica mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nueva puntuación
            group.MapPost("/", async (PuntuacionRequest puntuacion, IPuntuacionServices puntuacionService) =>
            {
                if (puntuacion == null)
                    return Results.BadRequest();

                var id = await puntuacionService.PostPuntuacion(puntuacion);

                return Results.Created($"/api/puntuacion/{id}", new
                {
                    message = "¡Puntuación creada exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nueva puntuación",
                Description = "Crea una nueva puntuación en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para actualizar una puntuación
            group.MapPut("/{id}", async (int id, PuntuacionRequest puntuacion, IPuntuacionServices puntuacionService) =>
            {
                if (puntuacion == null)
                    return Results.BadRequest();

                var result = await puntuacionService.PutPuntuacion(id, puntuacion);

                if (result == -1)
                    return Results.NotFound(new { message = "Puntuación no encontrada" });

                return Results.Ok(new
                {
                    message = "¡Puntuación actualizada exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar una puntuación",
                Description = "Actualiza los datos de una puntuación existente",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para eliminar una puntuación
            group.MapDelete("/{id}", async (int id, IPuntuacionServices puntuacionService) =>
            {
                var result = await puntuacionService.DeletePuntuacion(id);

                if (result == -1)
                    return Results.NotFound(new { message = "Puntuación no encontrada" });

                return Results.Ok(new
                {
                    message = "¡Puntuación eliminada exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar una puntuación",
                Description = "Elimina una puntuación del sistema mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });
        }
    }
}
