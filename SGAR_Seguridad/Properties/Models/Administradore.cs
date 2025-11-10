namespace SGAR_Seguridad.Properties.Models;

public partial class Administradore
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string EmailLaboral { get; set; } = null!;

    public string TelefonoLaboral { get; set; } = null!;

    public int IdUser { get; set; }

    public virtual Usuario IdUserNavigation { get; set; } = null!;
}
