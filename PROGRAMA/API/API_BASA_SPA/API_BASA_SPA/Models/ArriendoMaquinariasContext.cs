using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_BASA_SPA.Models;

public partial class ArriendoMaquinariasContext : DbContext
{
    public ArriendoMaquinariasContext()
    {
    }

    public ArriendoMaquinariasContext(DbContextOptions<ArriendoMaquinariasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Arriendo> Arriendos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Devolucion> Devolucions { get; set; }

    public virtual DbSet<EstadoArriendo> EstadoArriendos { get; set; }

    public virtual DbSet<EstadoFactura> EstadoFacturas { get; set; }

    public virtual DbSet<EstadoMantenimiento> EstadoMantenimientos { get; set; }

    public virtual DbSet<EstadoProducto> EstadoProductos { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Mantenimiento> Mantenimientos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<TipoCliente> TipoClientes { get; set; }

    public virtual DbSet<TipoMantencion> TipoMantencions { get; set; }

    public virtual DbSet<TipoProducto> TipoProductos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-L030J52;Database=ARRIENDO_MAQUINARIAS;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Arriendo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ARRIENDO__3214EC275B4B3A00");

            entity.ToTable("ARRIENDO", tb =>
                {
                    tb.HasTrigger("trg_AfterDeleteArriendo");
                    tb.HasTrigger("trg_AfterInsertArriendo");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdCliente)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("ID_CLIENTE");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.IdFactura).HasColumnName("ID_FACTURA");
            entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.Inicio)
                .HasColumnType("date")
                .HasColumnName("INICIO");
            entity.Property(e => e.Termino)
                .HasColumnType("date")
                .HasColumnName("TERMINO");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VALOR");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Arriendos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ARRIENDO__ID_CLI__571DF1D5");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Arriendos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ARRIENDO__ID_EST__5812160E");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.Arriendos)
                .HasForeignKey(d => d.IdFactura)
                .HasConstraintName("FK__ARRIENDO__ID_FAC__59063A47");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Arriendos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ARRIENDO__ID_PRO__5629CD9C");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Rut).HasName("PK__CLIENTE__CAF33259AF578FE5");

            entity.ToTable("CLIENTE");

            entity.Property(e => e.Rut)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("RUT");
            entity.Property(e => e.Direccion)
                .HasColumnType("text")
                .HasColumnName("DIRECCION");
            entity.Property(e => e.IdTipo).HasColumnName("ID_TIPO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Telefono).HasColumnName("TELEFONO");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CLIENTE__ID_TIPO__398D8EEE");
        });

        modelBuilder.Entity<Devolucion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DEVOLUCI__3214EC27FAA7A9D4");

            entity.ToTable("DEVOLUCION", tb =>
                {
                    tb.HasTrigger("trg_AfterDeleteDevolucion");
                    tb.HasTrigger("trg_AfterInsertDevolucion");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("FECHA");
            entity.Property(e => e.IdArriendo).HasColumnName("ID_ARRIENDO");

            entity.HasOne(d => d.IdArriendoNavigation).WithMany(p => p.Devolucions)
                .HasForeignKey(d => d.IdArriendo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DEVOLUCIO__ID_AR__5BE2A6F2");
        });

        modelBuilder.Entity<EstadoArriendo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTADO_A__3214EC2776A65FBA");

            entity.ToTable("ESTADO_ARRIENDO");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<EstadoFactura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTADO_F__3214EC2704BDC0CB");

            entity.ToTable("ESTADO_FACTURA");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<EstadoMantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTADO_M__3214EC27EA1B4617");

            entity.ToTable("ESTADO_MANTENIMIENTO");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<EstadoProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ESTADO_P__3214EC27931D7F98");

            entity.ToTable("ESTADO_PRODUCTO");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FACTURA__3214EC279B34D8F0");

            entity.ToTable("FACTURA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Bruto)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("BRUTO");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("DESCUENTO");
            entity.Property(e => e.FechaEmision)
                .HasColumnType("datetime")
                .HasColumnName("FECHA_EMISION");
            entity.Property(e => e.FechaExpiracion)
                .HasColumnType("date")
                .HasColumnName("FECHA_EXPIRACION");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("IVA");
            entity.Property(e => e.Neto)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("NETO");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("TOTAL");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FACTURA__ID_ESTA__3F466844");
        });

        modelBuilder.Entity<Mantenimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MANTENIM__3214EC275BBB0430");

            entity.ToTable("MANTENIMIENTO", tb =>
                {
                    tb.HasTrigger("trg_AfterDeleteMantenimiento");
                    tb.HasTrigger("trg_AfterUpdateMantenimiento");
                });

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Costo)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("COSTO");
            entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");
            entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");
            entity.Property(e => e.IdTipo).HasColumnName("ID_TIPO");
            entity.Property(e => e.Inicio)
                .HasColumnType("date")
                .HasColumnName("INICIO");
            entity.Property(e => e.Termino)
                .HasColumnType("date")
                .HasColumnName("TERMINO");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MANTENIMI__ID_ES__5070F446");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MANTENIMI__ID_PR__4E88ABD4");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Mantenimientos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MANTENIMI__ID_TI__4F7CD00D");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.NumSerie).HasName("PK__PRODUCTO__FC48906D20955D8B");

            entity.ToTable("PRODUCTO");

            entity.Property(e => e.NumSerie)
                .ValueGeneratedNever()
                .HasColumnName("NUM_SERIE");
            entity.Property(e => e.FechaAdq)
                .HasColumnType("date")
                .HasColumnName("FECHA_ADQ");
            entity.Property(e => e.IdEstado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ID_ESTADO");
            entity.Property(e => e.IdTipo).HasColumnName("ID_TIPO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VALOR");
            entity.Property(e => e.ValorResidual)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("VALOR_RESIDUAL");
            entity.Property(e => e.VidaUtil).HasColumnName("VIDA_UTIL");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRODUCTO__ID_EST__48CFD27E");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PRODUCTO__ID_TIP__49C3F6B7");
        });

        modelBuilder.Entity<TipoCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TIPO_CLI__3214EC271450DC57");

            entity.ToTable("TIPO_CLIENTE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<TipoMantencion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TIPO_MAN__3214EC2771908221");

            entity.ToTable("TIPO_MANTENCION");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasColumnType("text")
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<TipoProducto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TIPO_PRO__3214EC27064CA586");

            entity.ToTable("TIPO_PRODUCTO");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USUARIO__3214EC2760D86C7D");

            entity.ToTable("USUARIO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.Pass)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PASS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
