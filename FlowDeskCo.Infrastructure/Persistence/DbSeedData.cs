using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestateCo.Domain.Entities.CoreEntities;

namespace FlowDeskCo.Infrastructure.Persistence
{
    public static class DbSeedData
    {
        public static readonly DateTime dateTime = new DateTime(2025, 05, 01);
        public static readonly DateTime dateExipry = dateTime.AddDays(90);

        public static Role[] roles = new Role[] {
                new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Admin" },
                 new Role { Id = Guid.Parse("33333343-3333-3333-3333-333333333333"), Name = "User" }
            };

        public static Client[] clients = new Client[] {
                    new Client { Id = Guid.Parse("32233333-3333-3333-3333-333333333333"), Name = "Client A", CreatedAt = dateTime },
    new Client { Id = Guid.Parse("33233333-3333-3333-3333-333333333333"), Name = "Client B", CreatedAt = dateTime }
            };

        public static User[] users = new User[] {
               new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "user2@example.com", PhoneNumber = "2222222222", ClientId= Guid.Parse("32233333-3333-3333-3333-333333333333"),  CreatedAt=dateTime, IsActive=true },
                new User { Id = Guid.Parse("11111111-1111-1111-1151-111111211111"), Email = "user3@example.com", PhoneNumber = "3333333333", ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
                new User { Id = Guid.NewGuid(), Email = "admin@example.com",PasswordHash ="hashed-password", PhoneNumber = "4444444444" , ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
                new User { Id = Guid.Parse("11111111-1111-1111-1511-111112111111"), Email = "user5@example.com", PhoneNumber = "5555555555", ClientId = Guid.Parse("33233333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
                new User { Id = Guid.Parse("11111111-1111-1111-1511-111111221111"), Email = "user6@example.com", PhoneNumber = "6666666666", ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true }
        };

        public static Document[] documents = new Document[] {
                 new Document
                {
                    Id = Guid.Parse("33336333-3333-3333-3333-333333333333"),
                    FileName = "Sample Doc 1",
                    UploadedByUserId = Guid.Parse("11111111-1111-1111-1151-111111211111"),
                    LastUpdatedAt = dateTime,
                    ClientId= Guid.Parse("32233333-3333-3333-3333-333333333333"),
                    ExpiryDate = dateExipry
                },
                new Document
                {
                    Id = Guid.Parse("33333633-3333-3333-3333-333333333333"),
                    FileName = "Sample Doc 2",
                    ClientId= Guid.Parse("33233333-3333-3333-3333-333333333333"),
                    UploadedByUserId = Guid.Parse("11111111-1111-1111-1151-111111211111"),
                    LastUpdatedAt = dateTime,
                    ExpiryDate = dateExipry
                }
            };

        public static AuditLog[] auditLogs = new AuditLog[] {
    new AuditLog { Id = Guid.Parse("33333353-3333-3333-3333-333333333333"), Action = "UserLogin", PerformedBy = "aaa" , CreatedAt = dateTime },
new AuditLog { Id = Guid.Parse("33353333-3333-3333-3333-333333333333"), Action = "DocumentUpload", PerformedBy = "asdsda" , CreatedAt = dateTime }
};

        public static SharedLink[] sharedLinks = new SharedLink[] {
    new SharedLink
                {
                    Id = Guid.Parse("33333353-3334-3333-3333-333333333333"),
                    DocumentId = Guid.Parse("33333633-3333-3333-3333-333333333333"),
                    ShareCode = "https://example.com/share/abc123",
                    CreatedAt = dateTime,
                    ExpiryDate = dateExipry
                }
};



    //        modelBuilder.Entity<Role>().HasData(
    //new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Admin" },
    //new Role { Id = Guid.Parse("33333343-3333-3333-3333-333333333333"), Name = "User" });


    //        modelBuilder.Entity<Client>().HasData(
    //new Client { Id = Guid.Parse("32233333-3333-3333-3333-333333333333"), Name = "Client A", CreatedAt = dateTime },
    //new Client { Id = Guid.Parse("33233333-3333-3333-3333-333333333333"), Name = "Client B", CreatedAt = dateTime });


    //        modelBuilder.Entity<User>().HasData(
    //            new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "user2@example.com", PhoneNumber = "2222222222", ClientId= Guid.Parse("32233333-3333-3333-3333-333333333333"), RoleId= Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt=dateTime, IsActive=true },
    //            new User { Id = Guid.Parse("11111111-1111-1111-1151-111111211111"), Email = "user3@example.com", PhoneNumber = "3333333333", ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
    //            new User { Id = Guid.Parse("11111111-1111-1111-1111-111112111111"), Email = "user4@example.com", PhoneNumber = "4444444444" , ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
    //            new User { Id = Guid.Parse("11111111-1111-1111-1511-111112111111"), Email = "user5@example.com", PhoneNumber = "5555555555", ClientId = Guid.Parse("33233333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true },
    //            new User { Id = Guid.Parse("11111111-1111-1111-1511-111111221111"), Email = "user6@example.com", PhoneNumber = "6666666666", ClientId = Guid.Parse("32233333-3333-3333-3333-333333333333"), RoleId = Guid.Parse("33333333-3333-3333-3333-333333333333"), CreatedAt = dateTime, IsActive = true });

            
    //        modelBuilder.Entity<Document>().HasData(
    //            new Document
    //            {
    //                Id = Guid.Parse("33336333-3333-3333-3333-333333333333"),
    //                FileName = "Sample Doc 1",
    //                UploadedByUserId = Guid.Parse("11111111-1111-1111-1151-111111211111"),
    //                UploadedAt = dateTime,
    //                ExpiryDate = dateExipry
    //            },
    //            new Document
    //            {
    //                Id = Guid.Parse("33333633-3333-3333-3333-333333333333"),
    //                FileName = "Sample Doc 2",
    //                UploadedByUserId = Guid.Parse("11111111-1111-1111-1151-111111211111"),
    //                UploadedAt = dateTime,
    //                ExpiryDate = dateExipry
    //            });

    //        modelBuilder.Entity<AuditLog>().HasData(
    //            new AuditLog { Id = Guid.Parse("33333353-3333-3333-3333-333333333333"), Action = "UserLogin", PerformedBy = "aaa" , CreatedAt = dateTime },
    //            new AuditLog { Id = Guid.Parse("33353333-3333-3333-3333-333333333333"), Action = "DocumentUpload", PerformedBy = "asdsda" , CreatedAt = dateTime });

            
    //        modelBuilder.Entity<SharedLink>().HasData(
    //            new SharedLink
    //            {
    //                Id = Guid.Parse("33333353-3334-3333-3333-333333333333"),
    //                DocumentId = Guid.Parse("33333633-3333-3333-3333-333333333333"),
    //                ShareCode = "https://example.com/share/abc123",
    //                CreatedAt = dateTime,
    //                ExpiryDate = dateExipry
    //           });
        


    public static async Task SeedUsersAsync(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            var dateTime = DateTime.UtcNow;
            var rolesa = new[] { "User", "Admin" };
            foreach (var roleName in rolesa)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }

            var userRole = await roleManager.FindByNameAsync("User");
            var adminRole = await roleManager.FindByNameAsync("Admin");

            foreach (var user in DbSeedData.users)
            {
                var existingUser = await userManager.FindByIdAsync(user.Id.ToString());
                if (existingUser == null)
                {
                    var identityUser = new User
                    {
                        Id = user.Id,
                        UserName = user.Email,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        ClientId = user.ClientId,
                        IsActive = true,
                        CreatedAt = dateTime,
                        RoleId =adminRole.Id 
                    };

                    var result = await userManager.CreateAsync(identityUser, "DefaultP@ssword123");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(identityUser, "User");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        Console.WriteLine($"Failed to create user {user.Email}: {errors}");
                    }
                }
            }
        }
    }
}
