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
        public DbSet<Example> Examples { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
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
            var newDate = DateTime.Parse("Jan 1, 2022");

            modelBuilder.Entity<User>()
               .HasIndex(u => u.UserName)
               .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasDefaultValue(UserRole.Visitor);


            User user1 = new()
            {
                Uid = new Guid("7e085fa2-3726-4f90-a72e-7062e99d4e27"),
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Admin",
                UserName = "Admin",
                Email = "admin@teste.com",
                UserRole = UserRole.Admin,
                EmailConfirmed = true,
                LastAccess = newDate,
                CreatedAt = newDate,
                UpdatedAt = newDate,
            };
            User user2 = new()
            {
                Uid = new Guid("34c619e1-efeb-4e39-be1b-73f58a3bc443"),
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Cliente",
                UserName = "Cliente",
                Email = "cliente@teste.com",
                UserRole = UserRole.Default,
                EmailConfirmed = true,
                LastAccess = newDate,
                CreatedAt = newDate,
                UpdatedAt = newDate,
            };
            User user3 = new()
            {
                Uid = new Guid("14f091bc-cdb7-494a-bf4c-1f23da9244e5"),
                Password = "24-0B-E5-18-FA-BD-27-24-DD-B6-F0-4E-EB-1D-A5-96-74-48-D7-E8-31-C0-8C-8F-A8-22-80-9F-74-C7-20-A9",
                FullName = "Visitante",
                UserName = "Visitante",
                Email = "visitante@teste.com",
                UserRole = UserRole.Visitor,
                EmailConfirmed = true,
                LastAccess = newDate,
                CreatedAt = newDate,
                UpdatedAt = newDate,
            };
            modelBuilder.Entity<User>()
                .HasData(user1, user2, user3);

            Example example1 = new()
            {
                Id = 1,
                Name = "example 1",
                CreatedAt = newDate,
                UpdatedAt = newDate,
            };
            Example example2 = new()
            {
                Id = 2,
                Name = "example 2",
                CreatedAt = newDate,
                UpdatedAt = newDate,
            };
            modelBuilder.Entity<Example>()
                .HasData(example1, example2);
        }
    }
}
