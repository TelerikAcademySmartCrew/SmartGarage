using System;
using System.Security.Claims;
using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Contract;
using static SmartGarage.Common.Exceptions.ExceptionMessages;

namespace SmartGarage.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IEmailService emailService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PasswordGenerator passwordGenerator;
        
        public UsersService(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IEmailService emailService,
            IWebHostEnvironment webHostEnvironment,
            PasswordGenerator passwordGenerator
            )
        {
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
            this.emailService = emailService;
            this.webHostEnvironment = webHostEnvironment;
            this.passwordGenerator = passwordGenerator;
        }

        public async Task<ICollection<AppUser>> GetAll()
        {
            return await userManager.GetUsersInRoleAsync("Customer");
        }

        public async Task<ICollection<AppUser>> GetUsersInRoleAsync(string role)
        {
            var usersInRole = await userManager.GetUsersInRoleAsync(role);

            var userIds = usersInRole.Select(u => u.Id);

            var users = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Brand)
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Model)
                .Include(u => u.Vehicles)
                    .ThenInclude(v => v.Visits)
                        .ThenInclude(visit => visit.RepairActivities)
                            .ThenInclude(v => v.RepairActivityType).ToListAsync();

            return users;
        }

        public async Task<IdentityResult> CreateUser(AppUser appUser)
        {
            string randomPassword = passwordGenerator.Generate();

            // Get the wwwroot path
            var wwwrootPath = webHostEnvironment.WebRootPath;
            var filePath = Path.Combine(wwwrootPath, "AccountConfirmation.html")
                ?? throw new EntityNotFoundException("Email template not found.");

            string body;
            const string subject = "Welcome to Smart Garage!";

            using (var reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{Password}", randomPassword);
            body = body.Replace("{UserName}", appUser.Email);

            var userResult = await userManager.CreateAsync(appUser, randomPassword);
            if (!userResult.Succeeded)
            {
                throw new DuplicateEntityFoundException($"User already exists");
            }
            await userManager.AddToRoleAsync(appUser, "Customer");
            await applicationDbContext.SaveChangesAsync();

            await emailService.SendEmailAsync(appUser.Email, subject, body);

            return userResult;
        }

        public async Task<IdentityResult> CreateEmployee(AppUser appUser)
        {
            string randomPassword = "@Employee54";

            if (await UserWithEmailExists(appUser.Email))
            {
                throw new DuplicateEntityFoundException($"Email is already registered.");
            }

            var userResult = await userManager.CreateAsync(appUser, randomPassword);

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(appUser, "Employee");
                await applicationDbContext.SaveChangesAsync();
            }

            return userResult;
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
        public async Task<bool> Delete(AppUser user)
        {
            _ = await userManager.DeleteAsync(user)
                ?? throw new Exception("User not found");

            return true;
        }

        public async Task ResetPassword(AppUser user, string resetLink)
        {
            if (string.IsNullOrEmpty(resetLink))
            {
                throw new ArgumentNullException($"Password reset link in invalid");
            }

            // Get the wwwroot path
            var wwwrootPath = webHostEnvironment.WebRootPath;
            var filePath = Path.Combine(wwwrootPath, "PasswordResetConfirmation.html")
                ?? throw new EntityNotFoundException("Reset password email template not found.");

            string body;
            const string subject = "Password reset requested!";

            using (var reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{ResetLink}", resetLink);

            await emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task<IdentityResult> UpdateResetPassword(AppUser user, string resetToken, string newPassword, CancellationToken cancellationToken)
        {
            var result = await userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                applicationDbContext.SaveChanges();
            }

            return result;
        }
    }
}
