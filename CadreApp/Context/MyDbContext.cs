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

    public virtual DbSet<soldier_full> soldier_fulls { get; set; }

    public virtual DbSet<trip> trips { get; set; }

    public virtual DbSet<trip_location> trip_locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:cadreapp.database.windows.net,1433;Initial Catalog=cadreapp_db;Persist Security Info=False;User ID=cadreadmin;Password=\"IfYourNotFirst_YoureLAST\";MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip_Active>(entity =>
        {
            entity.ToView("Trip_Active");
        });

        modelBuilder.Entity<account>(entity =>
        {
            entity.HasOne(d => d.IDNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accounts_soldiers_ID_fk");
        });

        modelBuilder.Entity<solders_trip>(entity =>
        {
            entity.HasOne(d => d.soldersNavigation).WithMany().HasConstraintName("solders-trips_soldiers_ID_fk");

            entity.HasOne(d => d.trip).WithMany().HasConstraintName("solders-trips_trips_ID_fk");
        });

        modelBuilder.Entity<soldier>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("soldiers_pk");

            entity.Property(e => e.ID).ValueGeneratedNever();
        });

        modelBuilder.Entity<soldier_full>(entity =>
        {
            entity.ToView("soldier_full");
        });

        modelBuilder.Entity<trip>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("trips_pk");
        });

        modelBuilder.Entity<trip_location>(entity =>
        {
            entity.HasOne(d => d.trip).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trip-locations_trips_ID_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
