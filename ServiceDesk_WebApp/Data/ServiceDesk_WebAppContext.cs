using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServiceDesk_WebApp.Models;

namespace ServiceDesk_WebApp.Data
{
    public partial class ServiceDesk_WebAppContext : DbContext
    {
      
        public ServiceDesk_WebAppContext(DbContextOptions<ServiceDesk_WebAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.CreatedOn).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.ModifiedOn).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Password).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<ServiceDesk_WebApp.Models.Vendor> Vendor { get; set; }
    }
}
