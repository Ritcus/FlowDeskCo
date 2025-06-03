using FlowDeskCo.Application.Interfaces;
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
                entity.HasOne(x => x.Role).WithMany().HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(u => u.ClientId == _tenantProvider.GetTenantId());
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<SharedLink>(entity =>
            {
                entity.HasQueryFilter(s => s.ClientId == _tenantProvider.GetTenantId());
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(s => s.ExpiryDate).HasDefaultValueSql("DATEADD(hour, 24, GETUTCDATE())");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasQueryFilter(d => d.ClientId == _tenantProvider.GetTenantId());
                entity.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(d => d.ExpiryDate).HasDefaultValueSql("DATEADD(DAY, 60, GETUTCDATE())");
            });

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.HasQueryFilter(v => v.ClientId == _tenantProvider.GetTenantId());
                entity.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(s => s.ExpiryDate).HasDefaultValueSql("DATEADD(Minute, 15, GETUTCDATE())");
            });
            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasQueryFilter(v => v.ClientId == _tenantProvider.GetTenantId());
            });
            modelBuilder.Entity<NotificationLog>(entity =>
            {
                entity.HasQueryFilter(v => v.ClientId == _tenantProvider.GetTenantId());
            });


            // Configure relationships between CustomEntityDefinition and Fields
            modelBuilder.Entity<CustomEntityDefinition>()
                .HasMany(d => d.Fields)
                .WithOne()
                .HasForeignKey(f => f.CustomEntityDefinitionId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        =>
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=FlowDeskCo;Trusted_Connection=True;MultipleActiveResultSets=true;ConnectRetryCount=0").UseSeeding((context, _) =>
            {
                context.ChangeTracker.Clear();
                try { 
                // Seed Clients
                foreach (var client in DbSeedData.clients)
                {
                    if (!context.Set<Client>().Any(r => r.Id == client.Id))
                    {
                        context.Set<Client>().Add(client);
                    }
                }


                        // Seed Documents
                        foreach (var doc in DbSeedData.documents)
                {
                    if (context.Set<Document>().FirstOrDefault(r => r.Id == doc.Id) == null)
                    {
                        context.Set<Document>().Add(doc);
                    }
                }

                // Seed SharedLinks
                foreach (var link in DbSeedData.sharedLinks)
                {
                    if (!context.Set<SharedLink>().Any(r => r.Id == link.Id))
                    {
                        context.Set<SharedLink>().Add(link);
                    }
                }
                context.SaveChanges();
                }
                catch
                {
                    throw;
                }
            });
      

    }
}
