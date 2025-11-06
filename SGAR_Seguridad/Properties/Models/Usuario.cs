using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Email { get; set; } = null!;

    public string Dui { get; set; } = null!;

    public byte[]? Foto { get; set; }

    public string Password { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Ciudadano? Ciudadano { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual Operadore? Operadore { get; set; }

    public virtual ICollection<Puntuacion> Puntuacions { get; set; } = new List<Puntuacion>();

    public virtual Supervisore? Supervisore { get; set; }
}
