﻿using Microsoft.AspNetCore.Identity;
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
        private const string MasterAdminEmail = "master@admin.com";
        private const string MasterAdminPassword = "Admin@123";
        public static async Task<IdentityUser> Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var seededUser = await SeedUser(userManager, roleManager);
            return seededUser;
        }

        private static async Task<IdentityUser> SeedUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var user = await userManager.FindByEmailAsync(MasterAdminEmail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = MasterAdminEmail,
                    Email = MasterAdminEmail,
                    EmailConfirmed = true,
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
