using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfAppUserInterface.ModelData;

public partial class BridgeScalesContext : DbContext
{
    public BridgeScalesContext()
    {
    }

    public BridgeScalesContext(DbContextOptions<BridgeScalesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<Scale> Scales { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Weighing> Weighings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(Properties.Resources.SQLConnection);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A0465C2FB9C");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542B5BD29277C");

            entity.ToTable("Maintenance");

            entity.Property(e => e.MaintenanceId).HasColumnName("MaintenanceID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.MaintenanceType).HasMaxLength(50);
            entity.Property(e => e.ScaleId).HasColumnName("ScaleID");
            entity.Property(e => e.TechnicianId).HasColumnName("TechnicianID");

            entity.HasOne(d => d.Scale).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.ScaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maintenance_Scale");

            entity.HasOne(d => d.Technician).WithMany(p => p.Maintenances)
                .HasForeignKey(d => d.TechnicianId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Maintenance_Technician");
        });

        modelBuilder.Entity<Scale>(entity =>
        {
            entity.HasKey(e => e.ScaleId).HasName("PK__Scales__27D59546F07403FA");

            entity.HasIndex(e => e.SerialNumber, "UQ__Scales__048A00080458EE54").IsUnique();

            entity.Property(e => e.ScaleId).HasColumnName("ScaleID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.MaxCapacity).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACE7AF57F1");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E40DD836F8").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Weighing>(entity =>
        {
            entity.HasKey(e => e.WeighingId).HasName("PK__Weighing__86EA6967AFEFA7DB");

            entity.Property(e => e.WeighingId).HasColumnName("WeighingID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.GrossWeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OperatorId).HasColumnName("OperatorID");
            entity.Property(e => e.ScaleId).HasColumnName("ScaleID");
            entity.Property(e => e.TareWeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VehicleNumber).HasMaxLength(20);
            entity.Property(e => e.WeighingDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Weighings)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Weighings_Client");

            entity.HasOne(d => d.Operator).WithMany(p => p.Weighings)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Weighings_Operator");

            entity.HasOne(d => d.Scale).WithMany(p => p.Weighings)
                .HasForeignKey(d => d.ScaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Weighings_Scale");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
