using System;
using System.Collections.Generic;
using AirportApiServer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirportApiServer.Data.DbContexts;

public partial class AirportDbContext : DbContext
{
    public AirportDbContext()
    {
    }

    public AirportDbContext(DbContextOptions<AirportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<Leg> Legs { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<NextLeg> NextLegs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\AspNetCoreCourse;Initial Catalog=AirportApi;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasOne(d => d.CurrentLeg).WithMany(p => p.Flights).HasConstraintName("FK_Flight_Leg");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasOne(d => d.Flight).WithMany(p => p.Logs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Flight");

            entity.HasOne(d => d.Leg).WithMany(p => p.Logs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Log_Leg");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
