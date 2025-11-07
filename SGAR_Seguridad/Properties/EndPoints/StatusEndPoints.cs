using Microsoft.OpenApi.Models;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class StatusEndPoints
    {
        public static void add(this IEndpointRouteBuilder router)
        {
            var group = router.MapGroup("api/status").WithTags("Status");

            group.MapGet("/", async () =>
            {
                return Results.Ok(new { status = "API SGAR_Seguridad esta activa" });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Verificar el estado de la API",
                Description = "Este endpoint permite verificar si la API de SGAR_Seguridad está activa y funcionando correctamente."
            });
        }
    }
}
