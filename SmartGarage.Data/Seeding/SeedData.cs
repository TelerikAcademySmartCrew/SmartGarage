using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartGarage.Data.Models.VehicleModels;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Seeding
{
    public static class SeedData
    {
        public static async Task<IdentityUser> Initialize(UserManager<IdentityUser> userManager)
        {
            var seededUser = await SeedUsers(userManager);
            return seededUser;
        }

        private static async Task<IdentityUser> SeedUsers(UserManager<IdentityUser> userManager)
        {
            var userEmail = "bobi@admin.com";
            var user = await userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                };

                await userManager.CreateAsync(user, "Admin@123");
            }

            return user;
        }
    }
}
