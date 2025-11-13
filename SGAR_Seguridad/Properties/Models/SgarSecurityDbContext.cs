using Microsoft.EntityFrameworkCore;

namespace SGAR_Seguridad.Properties.Models;

public partial class SgarSecurityDbContext : DbContext
{
    public SgarSecurityDbContext()
    {
    }

    public SgarSecurityDbContext(DbContextOptions<SgarSecurityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administradore> Administradores { get; set; }

    public virtual DbSet<Ciudadano> Ciudadanos { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Organizacion> Organizacions { get; set; }

    public virtual DbSet<Puntuacion> Puntuacions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SolicitudesOperador> SolicitudesOperadors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administradore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Administ__3214EC07A93C700D");

            entity.HasIndex(e => e.Codigo, "UQ__Administ__06370DACDC00C9B4").IsUnique();

            entity.HasIndex(e => e.EmailLaboral, "UQ__Administ__3DA1AC845F6FC4A8").IsUnique();

            entity.HasIndex(e => e.TelefonoLaboral, "UQ__Administ__9F4A4975560D9E0B").IsUnique();

            entity.HasIndex(e => e.IdUser, "UQ__Administ__B7C92639112BC538").IsUnique();

            entity.Property(e => e.Codigo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.EmailLaboral)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoLaboral)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Administradore)
                .HasForeignKey<Administradore>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Administrador_Usuario");
        });

        modelBuilder.Entity<Ciudadano>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ciudadan__3214EC0740C62D4F");

            entity.HasIndex(e => e.IdUser, "UQ__Ciudadan__B7C926398DA23D42").IsUnique();

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Ciudadano)
                .HasForeignKey<Ciudadano>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudadano_Usuario");
        });

        modelBuilder.Entity<Operadore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operador__3214EC0737251B37");

            entity.HasIndex(e => e.CodigoOperador, "UQ__Operador__62F78FE3CCC05748").IsUnique();

            entity.HasIndex(e => e.IdUser, "UQ__Operador__B7C926399DF99C8E").IsUnique();

            entity.Property(e => e.CodigoOperador)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.Operadores)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operador_Organizacion");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Operadore)
                .HasForeignKey<Operadore>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operador_Usuario");
        });

        modelBuilder.Entity<Organizacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC07E9D544E4");

            entity.ToTable("Organizacion");

            entity.HasIndex(e => e.Email, "UQ__Organiza__A9D10534BF0BC4C1").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IdMunicipio)
               .HasMaxLength(250)
               .IsUnicode(false);
            entity.Property(e => e.NombreOrganizacion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Organizacions)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Organizacion_Rol");
        });

        modelBuilder.Entity<Puntuacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puntuaci__3214EC07837EEF38");

            entity.ToTable("Puntuacion");

            entity.Property(e => e.NombreAnonimo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Puntuacions)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Puntuacion_Usuario");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC074B1332AA");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__4F0B537F531F63CB").IsUnique();

            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SolicitudesOperador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Solicitu__3214EC07C3A5FAA7");

            entity.ToTable("SolicitudesOperador");

            entity.Property(e => e.Estado)
                .IsUnicode(false);
            entity.Property(e => e.FechaSolicitud)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdCiudadanoNavigation).WithMany(p => p.SolicitudesOperadors)
                .HasForeignKey(d => d.IdCiudadano)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Ciudadano");

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.SolicitudesOperadors)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Organizacion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07C733E939");

            entity.HasIndex(e => e.Telefono, "UQ__Usuarios__4EC50480231F8A19").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105347A32BC9B").IsUnique();

            entity.HasIndex(e => e.Dui, "UQ__Usuarios__C03671B991D218F2").IsUnique();

            entity.Property(e => e.Apellido)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Dui)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DUI");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
