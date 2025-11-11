using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.DTOs;
using SGAR_Seguridad.Properties.Services.Operadores;

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

            //EndPoint para crear nuevo registro de operador (JSON)
            group.MapPost("/", async (OperadorRequest operador, IOperadorServices operadorService) =>
            {
                try
                {
                    if (operador == null)
                        return Results.BadRequest();

                    var id = await operadorService.PostOperador(operador);

                    return Results.Created($"/api/operador/{id}", new
                    {
                        message = "¡Operador creado exitosamente!",
                        Id = id
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo Operador (JSON)",
                Description = "Crea un nuevo operador en el sistema",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para crear nuevo registro de operador con archivo de documento
            group.MapPost("/create", async (
                [Microsoft.AspNetCore.Mvc.FromForm] int idUser,
                [Microsoft.AspNetCore.Mvc.FromForm] string codigoOperador,
                [Microsoft.AspNetCore.Mvc.FromForm] int idVehiculo,
                [Microsoft.AspNetCore.Mvc.FromForm] int idOrganizacion,
                IFormFile? file,
                IOperadorServices operadorService) =>
            {
                try
                {
                    var operadorRequest = new CreateOperadorWithFileRequest
                    {
                        IdUser = idUser,
                        CodigoOperador = codigoOperador,
                        IdVehiculo = idVehiculo,
                        IdOrganizacion = idOrganizacion
                    };

                    var id = await operadorService.PostOperadorWithFile(operadorRequest, file);

                    return Results.Created($"/api/operador/{id}", new
                    {
                        message = "¡Operador creado exitosamente con documento!",
                        Id = id
                    });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo operador con documento (Form-Data)",
                Description = "Crea un nuevo operador en el sistema cargando directamente un documento de licencia. " +
                             "El documento es opcional. Formatos permitidos: PDF, DOC, DOCX. Tamaño máximo: 10MB",
            }).DisableAntiforgery();//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

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

            //EndPoint para actualizar un registro de operador (JSON)
            group.MapPut("/{id}", async (int id, OperadorRequest operadorUpdate, IOperadorServices operadorService) =>
            {
                try
                {
                    var result = await operadorService.PutOperador(id, operadorUpdate);
                    if (result == -1)
                        return Results.NotFound(new
                        {
                            message = "No se encontró el operador con el ID proporcionado."
                        });
                    else
                        return Results.Ok(new
                        {
                            message = "¡Operador actualizado exitosamente!",
                            Id = id,
                        });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar operador (JSON)",
                Description = "Actualiza la información de un operador existente",
            });//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            //EndPoint para actualizar operador con archivo de documento
            group.MapPut("/update/{id}", async (
                int id,
                [Microsoft.AspNetCore.Mvc.FromForm] int idUser,
                [Microsoft.AspNetCore.Mvc.FromForm] string codigoOperador,
                [Microsoft.AspNetCore.Mvc.FromForm] int idVehiculo,
                [Microsoft.AspNetCore.Mvc.FromForm] int idOrganizacion,
                IFormFile? file,
                IOperadorServices operadorService) =>
            {
                try
                {
                    var operadorRequest = new UpdateOperadorWithFileRequest
                    {
                        IdUser = idUser,
                        CodigoOperador = codigoOperador,
                        IdVehiculo = idVehiculo,
                        IdOrganizacion = idOrganizacion
                    };

                    var result = await operadorService.PutOperadorWithFile(id, operadorRequest, file);
                    if (result == -1)
                        return Results.NotFound(new
                        {
                            message = "No se encontró el operador con el ID proporcionado."
                        });
                    else
                        return Results.Ok(new
                        {
                            message = "¡Operador actualizado exitosamente!",
                            Id = id
                        });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar operador con documento (Form-Data)",
                Description = "Actualiza un operador existente con opción de actualizar o mantener el documento de licencia. " +
                             "- Si envías un archivo: Se actualiza el documento\n" +
                             "- Si NO envías archivo: Se mantiene el documento actual\n" +
                             "Formatos permitidos: PDF, DOC, DOCX. Tamaño máximo: 10MB",
            }).DisableAntiforgery();//.RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


        }
    }
}
