using FlowDeskCo.Application.Interfaces;
using FlowDeskCo.Domain.Entities.CoreEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestateCo.Domain.Entities.CoreEntities;
using RestateCo.Domain.Entities.CustomEntities;
using System;

namespace FlowDeskCo.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly IHostEnvironment _env;
        private readonly ITenantProvider _tenantProvider;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHostEnvironment env, ITenantProvider tenantProvider) : base(options) 
        {
            _env = env;
            _tenantProvider = tenantProvider;
        }
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<SharedLink> SharedLinks { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
        public DbSet<AuditLog> Logs { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<NotificationLog> NotificationLogs { get; set; } = null!;
        public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
        public DbSet<TenantSettings> TenantSettings { get; set; }

        public DbSet<CustomEntityDefinition> CustomEntityDefinitions { get; set; } = null!;
        public DbSet<CustomEntityFieldDefinition> CustomEntityFieldDefinitions { get; set; } = null!;
        public DbSet<CustomEntityRecord> CustomEntityRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity example:
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(x => x.Role).WithMany().HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<SharedLink>(entity =>
            {
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(s => s.ExpiryDate).HasDefaultValueSql("DATEADD(hour, 24, GETUTCDATE())");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(d => d.ExpiryDate).HasDefaultValueSql("DATEADD(DAY, 60, GETUTCDATE())");
            });

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.Property(v => v.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(v => v.ExpiryDate).HasDefaultValueSql("DATEADD(Minute, 15, GETUTCDATE())");
            });

            // Configure relationships between CustomEntityDefinition and Fields
            modelBuilder.Entity<CustomEntityDefinition>(entity =>
            {
                entity.HasMany(c => c.Fields)
                .WithOne()
                .HasForeignKey(f => f.CustomEntityDefinitionId);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
               
        }
    }
}
