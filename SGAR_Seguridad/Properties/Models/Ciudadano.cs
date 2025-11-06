using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Ciudadano
{
    public int Id { get; set; }

    public int IdZona { get; set; }

    public int IdUser { get; set; }

    public virtual Usuario IdUserNavigation { get; set; } = null!;
}
