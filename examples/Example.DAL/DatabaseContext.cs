using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.DAL;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure the DbContext if it hasn't been configured yet
        if (!optionsBuilder.IsConfigured)
        {
            // DbContext is not yet configured, configure it now
            optionsBuilder.UseSqlite("Data Source=<path_to_database_file>");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animal");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
