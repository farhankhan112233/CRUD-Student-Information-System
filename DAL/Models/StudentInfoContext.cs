using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CRUD.DAL.Models;

public partial class StudentInfoContext : DbContext
{
    public StudentInfoContext()
    {
    }

    public StudentInfoContext(DbContextOptions<StudentInfoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CandidateTable> CandidateTables { get; set; }

    public virtual DbSet<ClassTable> ClassTables { get; set; }

    public virtual DbSet<CourseTable> CourseTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=StudentInfo;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CandidateTable>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539B9C4A6B663F");

            entity.ToTable("CandidateTable");

            entity.Property(e => e.ClassId).HasColumnName("Class_Id");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.CandidateTables)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Candidate__Class__398D8EEE");
        });

        modelBuilder.Entity<ClassTable>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__ClassTab__CB1927C0B0805198");

            entity.ToTable("ClassTable");

            entity.Property(e => e.ClassName)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CourseTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CourseTa__3214EC07E66C24EC");

            entity.ToTable("CourseTable");

            entity.Property(e => e.CandidateId).HasColumnName("Candidate_Id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidate).WithMany(p => p.CourseTables)
                .HasForeignKey(d => d.CandidateId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CourseTable_CandidateTable");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
