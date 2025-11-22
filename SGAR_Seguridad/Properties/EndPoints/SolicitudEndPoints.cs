using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Solicitudes;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class SolicitudEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/solicitud").WithTags("Solicitudes");

            //EndPoint para obtener lista los registros de operadores paginados
            group.MapGet("/list", async (int? page, int? pageSize, ISolicitudServices oprService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var opr = await oprService.GetSolicitudes(currentPage, size);
                return Results.Ok(opr);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de solicitudes paginada",
                Description = "Retorna una lista paginada de solicitudes. Por defecto 10 registros por página.",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            //EndPoint para obtener una solicitud por id
            group.MapGet("/{id}", async (int id, ISolicitudServices solicitudService) =>
            {
                var getsolicitud = await solicitudService.GetSolicitud(id);
                if (getsolicitud == null)
                    return Results.NotFound();
                return Results.Ok(getsolicitud);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener una solicitud por ID",
                Description = "Obtiene una solicitud específico mediante su ID",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            //EndPoint para crear nuevo registro de solicitud
            group.MapPost("/", async (SolicitudRequest solictud, ISolicitudServices solicitudService) =>
            {
                if (solictud == null)
                    return Results.BadRequest();

                var id = await solicitudService.PostSolicitud(solictud);

                return Results.Created($"/api/solictud/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Solicitud creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nueva solicitud",
                Description = "Crea una nueva solicitud en el sistema",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });


            // EndPoint para eliminar un registro de solictud
            group.MapDelete("/{id}", async (int id, ISolicitudServices solicitudService) =>
            {
                var result = await solicitudService.DeleteSolicitud(id);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró la solicitud con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {
                        // Mensaje de éxito explícito
                        message = "¡Solicitud eliminado exitosamente!",
                        id = id
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar solicitud",
                Description = "Elimina una solicitud existente mediante su ID",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

            //EndPoint para actualizar una solicitud
            group.MapPut("/{id}", async (int id, SolicitudRequest solicitudUpdate, ISolicitudServices solicitudService) =>
            {
                var result = await solicitudService.PutSolicitud(id, solicitudUpdate);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró la solicitud con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {  // Mensaje de éxito explícito
                        message = "¡Solicitud actualizado exitosamente!",
                        Id = id,
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar solicitud",
                Description = "Actualiza la información de una solicitud existente",
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador, Operador, Ciudadano, Asociado, Organizacion" });

        }
    }
}