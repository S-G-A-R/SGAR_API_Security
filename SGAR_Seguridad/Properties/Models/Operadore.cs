using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Operadore
{
    public int Id { get; set; }

    public int IdUser { get; set; }

    public string CodigoOperador { get; set; } = null!;

    public int IdVehiculo { get; set; }

    public byte[]? LicenciaDoc { get; set; }

    public int IdOrganizacion { get; set; }

    public virtual Organizacion IdOrganizacionNavigation { get; set; } = null!;

    public virtual Usuario IdUserNavigation { get; set; } = null!;
}
