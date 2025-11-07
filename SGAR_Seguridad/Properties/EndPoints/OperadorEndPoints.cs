using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Ciudadanos;
using SGAR_Seguridad.Properties.Services.Operadores;
using SGAR_Seguridad.Properties.Services.Organizations;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class OperadorEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/operador").WithTags("Operadores");

            //EndPoint para obtener lista los registros de operadores paginados
            group.MapGet("/list", async (int? page, int? pageSize, IOperadorServices oprService) =>
            {
                var currentPage = page ?? 1;
                var size = pageSize ?? 10;

                if (currentPage < 1) currentPage = 1;
                if (size < 1) size = 10;
                if (size > 50) size = 50; // Límite máximo de registros por página

                var opr = await oprService.GetOperadores(currentPage, size);
                return Results.Ok(opr);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener lista de operadores paginada",
                Description = "Retorna una lista paginada de operadores. Por defecto 10 registros por página.",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para obtener organizacion por id
            group.MapGet("/{id}", async (int id, IOperadorServices operadorService) =>
            {
                var getoperador = await operadorService.GetOperador(id);
                if (getoperador == null)
                    return Results.NotFound();
                return Results.Ok(getoperador);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener un operador por ID",
                Description = "Obtiene un operador específico mediante su ID",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nuevo registro de organizaciones
            group.MapPost("/", async (OperadorRequest operador, IOperadorServices operadorService) =>
            {
                if (operador == null)
                    return Results.BadRequest();

                var id = await operadorService.PostOperador(operador);

                return Results.Created($"/api/operador/{id}", new
                {
                    // Mensaje de éxito explícito
                    message = "¡Operador creado exitosamente!",
                    Id = id
                });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo Operador",
                Description = "Crea un nuevo operador en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            // EndPoint para eliminar un registro de Operador por id
            group.MapDelete("/{id}", async (int id, IOperadorServices oprgService) =>
            {
                var result = await oprgService.DeleteOperador(id);
                if (result == -1)
                    return Results.NotFound(new
                    {
                        message = "No se encontró la operador con el ID proporcionado."
                    });
                else
                    return Results.Ok(new
                    {
                        // Mensaje de éxito explícito
                        message = "¡Operador eliminado exitosamente!",
                        id = id
                    });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar operador",
                Description = "Elimina una operador existente mediante su ID",
            });
        }
    }
}
