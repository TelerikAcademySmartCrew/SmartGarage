using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;

namespace SmartGarage.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly EmailService emailService;
        private readonly IConfiguration configuration;

        public UsersService(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext,
            EmailService emailService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.applicationDbContext = applicationDbContext;
            this.emailService = emailService;
            this.configuration = configuration;
        }

        public async Task<IdentityResult> Create(AppUser appUser)
        {
            try
            {
                // TODO : use the pass generator when ready

                // Generate a random password
                string randomPassword = "@User123";
                //string randomPassword = GenerateRandomPassword2();

                string subject = "Welcome to SmartGarage. Please use the possword sent with this email to login.";

                string body = $"Hello {appUser.Email}.\n\nYour account has been created." +
                    $"Please you the following password: {randomPassword} to login." +
                    $"You will be able to fill out your user profile data upon login";

                var userResult = await userManager.CreateAsync(appUser, randomPassword);

                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(appUser, "Customer");
                    await applicationDbContext.SaveChangesAsync();

                    // NOTE : toggle comment if you want to send emails
                    //await emailService.SendEmailAsync(appUser.Email, subject, body);
                }

                return userResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AppUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Brand)
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Model)
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Visits)
                        .ThenInclude(visit => visit.RepairActivities)
                            .ThenInclude(v => v.RepairActivityType)
                .FirstOrDefaultAsync(u => u.UserName == user.Identity.Name)
                ?? throw new EntityNotFoundException("User not found.");

            //return await userManager.GetUserAsync(user)
            //    ?? throw new EntityNotFoundException($"User not found.");
        }

        public async Task<AppUser> GetByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email)
                ?? throw new Exception("User not found");
        }

        public async Task<bool> UserWithEmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        public async Task<AppUser> Update(AppUser appUser)
        {
            _ = await userManager.UpdateAsync(appUser);

            var updatedUser = await userManager.FindByIdAsync(appUser.Id)
                ?? throw new EntityNotFoundException("User not found.");

            return updatedUser;
        }

        private string GenerateRandomPassword()
        {
            // Implement your logic to generate a random password here
            // For example, you can use a library like "System.Security.Cryptography.RandomNumberGenerator"
            // to generate a secure random password.

            // For simplicity, a basic example is provided below:
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new string(Enumerable.Repeat(chars, 12) // Change 8 to the desired password length
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return password;
        }

        // Function to generate a random password with at least one non-alphanumeric character
        private string GenerateRandomPassword2()
        {
            const string alphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string specialChars = "@#$%&";

            var random = new Random();

            // Include at least one non-alphanumeric character
            var password = new string(alphanumericChars[random.Next(alphanumericChars.Length)].ToString()) +
                           new string(Enumerable.Repeat(alphanumericChars + specialChars, 11)
                               .Select(s => s[random.Next(s.Length)])
                               .ToArray());

            return password; // + "!@#";
        }
    }
}
