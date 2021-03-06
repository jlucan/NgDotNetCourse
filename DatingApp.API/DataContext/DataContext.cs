﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DatingApp.API.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Value> Value { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Fname)
                    .HasColumnName("FName")
                    .HasMaxLength(30);

                entity.Property(e => e.Lname)
                    .HasColumnName("LName")
                    .HasMaxLength(30);

                entity.Property(e => e.Mname)
                    .HasColumnName("MName")
                    .HasMaxLength(30);

                entity.Property(e => e.PswdHash).HasMaxLength(200);

                entity.Property(e => e.PswdSalt).HasMaxLength(200);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30);

            });

            modelBuilder.Entity<Value>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });
        }
    }
}
