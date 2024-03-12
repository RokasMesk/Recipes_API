using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recipe.Models;

namespace Recipe.Data
{
    public class AuthDbContext:IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext (DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var userRoleId = "46385bc4-f2f5-4557-a54f-5a555ddbb448";
            var adminRoleId = "322ffb03-c7aa-463f-a4e6-15bbb2da3979";
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper(),
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = adminRoleId
                }
            };
            // Seeding roles
            builder.Entity<IdentityRole>().HasData(roles);
            // Creating admin
            var adminUserId = "fe6995fd-ed28-441f-b4cd-378ec0c046d9";
            var admin = new ApplicationUser()
            {
                Id = adminUserId,
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper()
            };
            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "Admin123#");
            builder.Entity<ApplicationUser>().HasData(admin);
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId,
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
