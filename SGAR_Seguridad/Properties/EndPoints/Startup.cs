using Microsoft.OpenApi.Models;

namespace SGAR_Seguridad.Properties.EndPoints
{
    public static class Startup
    {
        public static void UseEndPoints(this WebApplication app)
        {
            StatusEndPoints.add(app);
            UserEndPoints.add(app);
            OrganizacionEndPoints.add(app);
            CiudadanoEndPoints.add(app);
            AdministradorEndPoints.add(app);
            OperadorEndPoints.add(app);
        }
    }
}