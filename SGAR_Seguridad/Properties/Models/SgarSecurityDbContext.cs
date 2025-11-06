using System;
using System.Collections.Generic;
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

    public virtual DbSet<Ciudadano> Ciudadanos { get; set; }

    public virtual DbSet<Operadore> Operadores { get; set; }

    public virtual DbSet<Organizacion> Organizacions { get; set; }

    public virtual DbSet<Puntuacion> Puntuacions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supervisore> Supervisores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ciudadano>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ciudadan__3214EC07D48E0664");

            entity.HasIndex(e => e.IdUser, "UQ__Ciudadan__B7C92639ACA21605").IsUnique();

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Ciudadano)
                .HasForeignKey<Ciudadano>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudadano_Usuario");
        });

        modelBuilder.Entity<Operadore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operador__3214EC075D867C1A");

            entity.HasIndex(e => e.CodigoOperador, "UQ__Operador__62F78FE3570A1572").IsUnique();

            entity.HasIndex(e => e.IdUser, "UQ__Operador__B7C92639A68792B3").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Organiza__3214EC07F1A7CB3E");

            entity.ToTable("Organizacion");

            entity.HasIndex(e => e.Email, "UQ__Organiza__A9D105340B98D45C").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
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
        });

        modelBuilder.Entity<Puntuacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Puntuaci__3214EC07AA5224A1");

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
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0704A653E2");

            entity.HasIndex(e => e.NombreRol, "UQ__Roles__4F0B537F1F770135").IsUnique();

            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Supervisore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supervis__3214EC071BA412DE");

            entity.HasIndex(e => e.Codigo, "UQ__Supervis__06370DAC8F013BC8").IsUnique();

            entity.HasIndex(e => e.EmailLaboral, "UQ__Supervis__3DA1AC8468131CA1").IsUnique();

            entity.HasIndex(e => e.TelefonoLaboral, "UQ__Supervis__9F4A49755C25E856").IsUnique();

            entity.HasIndex(e => e.IdUser, "UQ__Supervis__B7C92639F5A93281").IsUnique();

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

            entity.HasOne(d => d.IdOrganizacionNavigation).WithMany(p => p.Supervisores)
                .HasForeignKey(d => d.IdOrganizacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supervisor_Organizacion");

            entity.HasOne(d => d.IdUserNavigation).WithOne(p => p.Supervisore)
                .HasForeignKey<Supervisore>(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Supervisor_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC07175E2E04");

            entity.HasIndex(e => e.Telefono, "UQ__Usuarios__4EC50480EEFEE6F7").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534A867AD25").IsUnique();

            entity.HasIndex(e => e.Dui, "UQ__Usuarios__C03671B9CF25C793").IsUnique();

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
