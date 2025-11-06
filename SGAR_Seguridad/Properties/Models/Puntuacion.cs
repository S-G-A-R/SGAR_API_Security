using System;
using System.Collections.Generic;

namespace SGAR_Seguridad.Properties.Models;

public partial class Puntuacion
{
    public int Id { get; set; }

    public int Puntos { get; set; }

    public string? NombreAnonimo { get; set; }

    public int? IdUser { get; set; }

    public virtual Usuario? IdUserNavigation { get; set; }
}
