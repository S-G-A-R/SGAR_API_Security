using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class SolicitudesOperador
{
    public int Id { get; set; }

    public int IdCiudadano { get; set; }

    public int IdOrganizacion { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Ciudadano IdCiudadanoNavigation { get; set; } = null!;

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;
}
