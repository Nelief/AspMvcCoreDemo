using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MvcCoreDemo.Data
{
    public enum ROLES
    {
        USER,
        ADMIN,
        SUPERADMIN
    }

    public class AuthDbContext(DbContextOptions<AuthDbContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles
            var AdminRoleId = "4323d2b3-f3c3-4def-bbb2-7a2d7a479e57";
            var SuperAdminRoleId = "815e46a2-7e60-4d4f-baeb-6928ac811376";
            var UserRoleId = "af7a0294-0247-4e9b-8dad-8924941dd178";

            var roles = new List<IdentityRole>
            {
                new() {
                    Name = ROLES.ADMIN.ToString(),
                    NormalizedName = ROLES.ADMIN.ToString(),
                    Id = AdminRoleId,
                    ConcurrencyStamp = AdminRoleId
                },
                new()
                {
                    Name = ROLES.SUPERADMIN.ToString(),
                    NormalizedName = ROLES.SUPERADMIN.ToString(),
                    Id = SuperAdminRoleId,
                    ConcurrencyStamp = SuperAdminRoleId
                },
                new()
                {
                    Name = ROLES.USER.ToString(),
                    NormalizedName = ROLES.USER.ToString(),
                    Id = UserRoleId,
                    ConcurrencyStamp = UserRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);


            //Seed Super Admin user

            var SuperAdminUserId = "77733658-ce6c-49e3-803b-8ffb2dbebe05";

            var SuperAdminUser = new IdentityUser
            {
                UserName = "superAdmin",
                Email = "superAdmin@gmail.com",
                NormalizedEmail = "superAdmin@gmail.com".ToUpper(),
                NormalizedUserName = "superAdmin".ToUpper(),
                Id = SuperAdminUserId
            };

            SuperAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(SuperAdminUser, "admin");

            builder.Entity<IdentityUser>().HasData(SuperAdminUser);

            //Give roles to SA user 
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new() {
                    RoleId =UserRoleId,
                    UserId =SuperAdminUserId
                },
                new() {
                    RoleId =SuperAdminRoleId,
                    UserId =SuperAdminUserId
                },
                new() {
                    RoleId =AdminRoleId,
                    UserId =SuperAdminUserId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }

    }
}
