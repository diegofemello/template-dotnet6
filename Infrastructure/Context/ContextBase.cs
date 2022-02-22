using Microsoft.EntityFrameworkCore;
using Domain.Model;
using System;
using Domain.Model.Base;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace Infrastructure.Context
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Example> Examples { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();

            //return base.SaveChangesAsync();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            IEnumerable<EntityEntry> entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entity in entities)
            {
                DateTime now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedAt = now;
                }
                ((BaseEntity)entity.Entity).UpdatedAt = now;
            }

            IEnumerable<EntityEntry> users = ChangeTracker.Entries()
                .Where(x => x.Entity is User && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (EntityEntry entity in users)
            {
                DateTime now = DateTime.UtcNow;

                if (entity.State == EntityState.Added)
                {
                    ((User)entity.Entity).CreatedAt = now;
                }
                ((User)entity.Entity).UpdatedAt = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique(true);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("Cliente");

            modelBuilder.Entity<UserRole>()
                .HasIndex(u => u.Name)
                .IsUnique(true);

            UserRole userRole1 = new()
            {
                Name = "Cliente",
            };
            UserRole userRole2 = new()
            {
                Name = "Tecnico",
            };
            UserRole userRole3 = new()
            {
                Name = "Admin",
            };
            modelBuilder.Entity<UserRole>()
                .HasData(userRole1, userRole2, userRole3);


            User user1 = new()
            {
                Uid = new Guid("7e085fa2-3726-4f90-a72e-7062e99d4e27"),
                UserName = "Admin",
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Admin da Silva",
                Email = "admin@teste.com",
                Role = userRole3.Name,
                EmailConfirmed = true,
                LastAccess = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            User user2 = new()
            {
                Uid = new Guid("34c619e1-efeb-4e39-be1b-73f58a3bc443"),
                UserName = "cliente",
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Cliente",
                Email = "cliente@teste.com",
                Role = userRole1.Name,
                EmailConfirmed = true,
                LastAccess = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            User user3 = new()
            {
                Uid = new Guid("14f091bc-cdb7-494a-bf4c-1f23da9244e5"),
                UserName = "tecnico",
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Tecnico",
                Email = "tecnico@teste.com",
                Role = userRole2.Name,
                EmailConfirmed = true,
                LastAccess = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            modelBuilder.Entity<User>()
                .HasData(user1, user2, user3);

            Example example1 = new()
            {
                Id = 1,
                Name = "example 1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            Example example2 = new()
            {
                Id = 2,
                Name = "example 2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            modelBuilder.Entity<Example>()
                .HasData(example1, example2);
        }
    }
}
