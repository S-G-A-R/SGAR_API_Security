using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Role
{
    public int Id { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Organizacion> Organizacions { get; set; } = new List<Organizacion>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
