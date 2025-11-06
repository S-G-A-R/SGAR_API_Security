using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Organizacion
{
    public int Id { get; set; }

    public int IdMunicipio { get; set; }

    public string NombreOrganizacion { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Operadore> Operadores { get; set; } = new List<Operadore>();

    public virtual ICollection<Supervisore> Supervisores { get; set; } = new List<Supervisore>();
}
