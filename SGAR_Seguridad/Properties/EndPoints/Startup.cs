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
            OperadorEndPoints.add(app);
            SolicitudEndPoints.add(app);
            PuntuacionEndPoints.add(app);
            RoleEndPoints.add(app);
        }
    }
}