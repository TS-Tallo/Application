using System;
using System.Collections.Generic;
using CadreApp.TableEntities;
using Microsoft.EntityFrameworkCore;

namespace CadreApp.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Trip_Active> Trip_Actives { get; set; }

    public virtual DbSet<account> accounts { get; set; }

    public virtual DbSet<solders_trip> solders_trips { get; set; }

    public virtual DbSet<soldier> soldiers { get; set; }

    public virtual DbSet<trip> trips { get; set; }

    public virtual DbSet<trip_location> trip_locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:cadreapp.database.windows.net,1433;Initial Catalog=cadreapp_db;Persist Security Info=False;User ID=cadreadmin;Password=IfYourNotFirst_YoureLAST;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip_Active>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Trip_Active");

            entity.Property(e => e.ID)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.time_in).HasColumnType("datetime");
            entity.Property(e => e.time_out).HasColumnType("datetime");
        });

        modelBuilder.Entity<account>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.password).HasMaxLength(256);
            entity.Property(e => e.username)
                .HasMaxLength(32)
                .IsUnicode(false);

            entity.HasOne(d => d.IDNavigation).WithMany()
                .HasForeignKey(d => d.ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accounts_soldiers_ID_fk");
        });

        modelBuilder.Entity<solders_trip>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("solders-trips");

            entity.Property(e => e.trip_id)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.soldersNavigation).WithMany()
                .HasForeignKey(d => d.solders)
                .HasConstraintName("solders-trips_soldiers_ID_fk");

            entity.HasOne(d => d.trip).WithMany()
                .HasForeignKey(d => d.trip_id)
                .HasConstraintName("solders-trips_trips_ID_fk");
        });

        modelBuilder.Entity<soldier>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("soldiers_pk");

            entity.Property(e => e.ID).ValueGeneratedNever();
            entity.Property(e => e.building)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.company)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.name_first)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.name_last)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.platoon)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.rank)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<trip>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("trips_pk");

            entity.Property(e => e.ID)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.time_in).HasColumnType("datetime");
            entity.Property(e => e.time_out).HasColumnType("datetime");
        });

        modelBuilder.Entity<trip_location>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("trip-locations");

            entity.Property(e => e.location)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.trip_id)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.trip).WithMany()
                .HasForeignKey(d => d.trip_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip-locations_trips_ID_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
