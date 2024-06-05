using System;
using System.Collections.Generic;
using BalanceScoreCard.Models;
using Microsoft.EntityFrameworkCore;

namespace BalanceScoreCard.Data;

public partial class CerveceriaDwContext : DbContext
{
    public CerveceriaDwContext()
    {
    }

    public CerveceriaDwContext(DbContextOptions<CerveceriaDwContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DimDistribuidor> DimDistribuidors { get; set; }

    public virtual DbSet<DimEmpleado> DimEmpleados { get; set; }

    public virtual DbSet<DimProducto> DimProductos { get; set; }

    public virtual DbSet<DimTiempo> DimTiempos { get; set; }

    public virtual DbSet<DimVentum> DimVenta { get; set; }

    public virtual DbSet<Hecho> Hechoes { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:cadenaConexion");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DimDistribuidor>(entity =>
        {
            entity.HasKey(e => e.IdDistribuidor).HasName("PK__Dim_Dist__4D1FF6AE180B4043");

            entity.ToTable("Dim_Distribuidor");

            entity.Property(e => e.IdDistribuidor)
                .ValueGeneratedNever()
                .HasColumnName("Id_Distribuidor");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DimEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Dim_Empl__74056223A4AB10BF");

            entity.ToTable("Dim_Empleado");

            entity.Property(e => e.IdEmpleado)
                .ValueGeneratedNever()
                .HasColumnName("Id_Empleado");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DimProducto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Dim_Prod__2085A9CFEBB9B33A");

            entity.ToTable("Dim_Producto");

            entity.Property(e => e.IdProducto)
                .ValueGeneratedNever()
                .HasColumnName("Id_Producto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioUnit)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Precio_Unit");
        });

        modelBuilder.Entity<DimTiempo>(entity =>
        {
            entity.HasKey(e => e.Fecha).HasName("PK__Dim_Tiem__B30C8A5FAD390641");

            entity.ToTable("Dim_Tiempo");
        });

        modelBuilder.Entity<DimVentum>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("PK__Dim_Vent__B3C9EABD5444C114");

            entity.ToTable("Dim_Venta");

            entity.Property(e => e.IdVenta)
                .ValueGeneratedNever()
                .HasColumnName("Id_Venta");
            entity.Property(e => e.TotalVenta)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Venta");
        });

        modelBuilder.Entity<Hecho>(entity =>
        {
            entity.HasKey(e => e.IdHecho).HasName("PK__Hecho__EA68E4F9DDF9786E");

            entity.ToTable("Hecho");

            entity.Property(e => e.IdHecho).HasColumnName("Id_Hecho");
            entity.Property(e => e.IdDistribuidor).HasColumnName("Id_Distribuidor");
            entity.Property(e => e.IdEmpleado).HasColumnName("Id_Empleado");
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.IdTiempo).HasColumnName("Id_Tiempo");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.SubTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Sub_Total");

            entity.HasOne(d => d.DistribuidorNavigation).WithMany(p => p.Hechoes)
                .HasForeignKey(d => d.IdDistribuidor)
                .HasConstraintName("FK__Hecho__Id_Distri__440B1D61");

            entity.HasOne(d => d.EmpleadoNavigation).WithMany(p => p.Hechoes)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK__Hecho__Id_Emplea__44FF419A");

            entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.Hechoes)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK__Hecho__Id_Produc__412EB0B6");

            entity.HasOne(d => d.TiempoNavigation).WithMany(p => p.Hechoes)
                .HasForeignKey(d => d.IdTiempo)
                .HasConstraintName("FK__Hecho__Id_Tiempo__4316F928");

            entity.HasOne(d => d.VentaNavigation).WithMany(p => p.Hechoes)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK__Hecho__Id_Venta__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
