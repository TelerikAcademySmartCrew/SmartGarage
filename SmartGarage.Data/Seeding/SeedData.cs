using Microsoft.AspNetCore.Identity;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Data.Seeding
{
    public static class SeedData
    {
        private const string MasterAdminEmail = "master@admin.com";
        private const string MasterAdminPassword = "Admin@123";
        public static async Task<IdentityUser> Initialize(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var seededUser = await SeedUser(userManager, roleManager);
            return seededUser;
        }

        private static async Task<IdentityUser> SeedUser(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = await userManager.FindByEmailAsync(MasterAdminEmail);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = MasterAdminEmail,
                    Email = MasterAdminEmail,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow
                };

                await userManager.CreateAsync(user, MasterAdminPassword);

                await roleManager.CreateAsync(new IdentityRole("Employee"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Customer"));

                await userManager.AddToRoleAsync(user, "Admin");

            }

            return user;
        }
    }
}
