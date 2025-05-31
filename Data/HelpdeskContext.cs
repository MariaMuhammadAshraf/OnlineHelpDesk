using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineHelpDesk.Models;

namespace OnlineHelpDesk.Data;

public partial class HelpdeskContext : DbContext
{
    public HelpdeskContext()
    {
    }

    public HelpdeskContext(DbContextOptions<HelpdeskContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<ContactMessage> ContactMessages { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserRegistration> UserRegistrations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-EUUJM3C\\SQLEXPRESS ;Initial Catalog=helpdesk;Persist Security Info=False;User ID=maria ;Password=123456 ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__aboutus__3214EC07430DD2D5");

            entity.ToTable("aboutus");

            entity.Property(e => e.ImagePath).HasMaxLength(255);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactM__3214EC07331BB7BC");

            entity.ToTable("ContactMessage");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Subject).HasMaxLength(150);
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__Faciliti__5FB08A7462EA66DB");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FacilityName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07490BA863");

            entity.Property(e => e.ClientName).HasMaxLength(255);
            entity.Property(e => e.ImagePath).HasMaxLength(255);
            entity.Property(e => e.Profession).HasMaxLength(255);
        });

        modelBuilder.Entity<UserRegistration>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__USER_REG__F3BEEBFFE93C42F6");

            entity.ToTable("USER_REGISTRATION");

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("FIRST_NAME");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("LAST_NAME");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_EMAIL");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_PASSWORD");
            entity.Property(e => e.UserRole)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_ROLE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
