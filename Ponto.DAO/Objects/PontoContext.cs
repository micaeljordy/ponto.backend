using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ponto.DAL.Objects;

public partial class PontoContext : DbContext
{
    public PontoContext()
    {
    }

    public PontoContext(DbContextOptions<PontoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entry> Entries { get; set; }

    public virtual DbSet<Register> Registers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL(System.Environment.GetEnvironmentVariable("PONTO_API_CONNECTION_STRING"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entry");

            entity.HasIndex(e => e.RegisterId, "RegisterId");

            entity.Property(e => e.PunchDateTime).HasMaxLength(50);

            entity.HasOne(d => d.Register).WithMany(p => p.Entries)
                .HasForeignKey(d => d.RegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entry_ibfk_1");
        });

        modelBuilder.Entity<Register>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("register");

            entity.Property(e => e.EmployeeName).HasMaxLength(50);
            entity.Property(e => e.PunchDate).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
