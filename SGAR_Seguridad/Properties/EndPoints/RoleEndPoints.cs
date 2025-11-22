using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using SGAR_Seguridad.Properties.Services.Rols;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class RoleEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/rol").WithTags("Roles");

            //EndPoint para obtener todos lo registros de rol
            group.MapGet("/", async (IRoleService rolServices) =>
            {
                var rols = await rolServices.GetRoles();
                return Results.Ok(rols);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener roles",
                Description = "Lista de todos los roles",

            });

        }
    }
}
