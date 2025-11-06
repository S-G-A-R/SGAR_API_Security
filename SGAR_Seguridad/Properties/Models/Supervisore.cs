using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Supervisore
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string EmailLaboral { get; set; } = null!;

    public string TelefonoLaboral { get; set; } = null!;

    public int IdUser { get; set; }

    public int IdOrganizacion { get; set; }

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUserNavigation { get; set; } = null!;
}
