using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Ciudadano
{
    public int Id { get; set; }

    public string IdZona { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual Usuario IdUserNavigation { get; set; } = null!;

    public virtual ICollection<SolicitudesOperador> SolicitudesOperadors { get; set; } = new List<SolicitudesOperador>();
}
