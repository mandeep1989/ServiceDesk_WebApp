using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceDesk_WebApp.Models;

namespace ServiceDesk_WebApp.Data
{
    public partial class ServiceDesk_WebAppContext : DbContext
    {
        public ServiceDesk_WebAppContext()
        {
        }

        public ServiceDesk_WebAppContext(DbContextOptions<ServiceDesk_WebAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChangePasswordRequest> ChangePasswordRequests { get; set; }
        public virtual DbSet<EscalationMatrix> EscalationMatrices { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChangePasswordRequest>(entity =>
            {
                entity.ToTable("ChangePasswordRequest");

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.ModifiedOn).IsRequired();
            });

            modelBuilder.Entity<EscalationMatrix>(entity =>
            {
                entity.ToTable("EscalationMatrix");

                entity.Property(e => e.CompanyEmail).IsRequired();

                entity.Property(e => e.CompanyName).IsRequired();

                entity.Property(e => e.CompanyPhone).IsRequired();

                entity.Property(e => e.ContactEmail).IsRequired();

                entity.Property(e => e.ContactName).IsRequired();

                entity.Property(e => e.ContactPhone).IsRequired();

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.ModifiedOn).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.ModifiedOn).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Vendor");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.Currency).IsRequired();

                entity.Property(e => e.ModifiedOn).IsRequired();

                entity.Property(e => e.Poremarks)
                    .IsRequired()
                    .HasColumnName("PORemarks");

                entity.Property(e => e.ResidencyStatus).IsRequired();

                entity.Property(e => e.VendorName).IsRequired();

                entity.Property(e => e.VendorNo).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
