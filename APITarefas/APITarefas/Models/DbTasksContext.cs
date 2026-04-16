using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace APITarefas.Models;

public partial class DbTasksContext : DbContext
{
    public DbTasksContext()
    {
    }

    public DbTasksContext(DbContextOptions<DbTasksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=dbTasks;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FK_SubTasks_Tasks")
                .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Data).HasColumnType("datetime");
            entity.Property(e => e.Descrição).HasColumnType("text");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
