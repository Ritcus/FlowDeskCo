using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CoreEntities;
using RestateCo.Domain.Entities.CustomEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestateCo.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<SharedLink> SharedLinks { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
        public DbSet<AuditLog> Logs { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<NotificationLog> NotificationLogs { get; set; } = null!;
        public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;

        public DbSet<CustomEntityDefinition> CustomEntityDefinitions { get; set; } = null!;
        public DbSet<CustomEntityFieldDefinition> CustomEntityFieldDefinitions { get; set; } = null!;
        public DbSet<CustomEntityRecord> CustomEntityRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity example:
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PhoneNumber).HasMaxLength(20);
                // Add other configurations and indexes
            });

            // Configure relationships between CustomEntityDefinition and Fields
            modelBuilder.Entity<CustomEntityDefinition>()
                .HasMany(d => d.Fields)
                .WithOne()
                .HasForeignKey(f => f.CustomEntityDefinitionId);

        }
    }
}
