using System;
using System.Collections.Generic;
using CadreApp.TableEntities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
    
    Config loadedConfig = ConfigurationManager.LoadConfig();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(loadedConfig.ConnectionString);

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
                entity.HasKey(a => new { a.ID});
        });

        modelBuilder.Entity<solders_trip>(entity =>
        {
            entity.HasOne(d => d.soldersNavigation).WithMany().HasConstraintName("solders-trips_soldiers_ID_fk");
            entity.HasKey(st => new { st.trip_id, st.solders });
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
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
