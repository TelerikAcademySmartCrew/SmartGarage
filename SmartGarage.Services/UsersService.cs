using System.Security.Claims;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Contract;
using SmartGarage.Services.Contracts;
using SmartGarage.Common.Exceptions;

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
            return await this.userManager.GetUsersInRoleAsync("Customer");
        }

        public async Task<ICollection<AppUser>> GetUsersInRoleAsync(string role)
        {
            var usersInRole = await this.userManager.GetUsersInRoleAsync(role);

            var userIds = usersInRole.Select(u => u.Id);

            var users = await this.userManager.Users
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
            var randomPassword = this.passwordGenerator.Generate();

            var wwwrootPath = this.webHostEnvironment.WebRootPath;
            var filePath = Path.Combine(wwwrootPath, "AccountConfirmation.html")
                ?? throw new EntityNotFoundException("Email template not found.");

            var body = string.Empty;
            const string subject = "Welcome to Smart Garage!";

            using (var reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{Password}", randomPassword);
            body = body.Replace("{UserName}", appUser.Email);

            var userResult = await this.userManager.CreateAsync(appUser, randomPassword);

            if (!userResult.Succeeded)
            {
                throw new DuplicateEntityFoundException($"User already exists");
            }

            await this.userManager.AddToRoleAsync(appUser, "Customer");
            await this.applicationDbContext.SaveChangesAsync();

            await this.emailService.SendEmailAsync(appUser.Email, subject, body);

            return userResult;
        }

        public async Task<IdentityResult> CreateEmployee(AppUser appUser)
        {
            var randomPassword = "@Employee54";

            if (await UserWithEmailExists(appUser.Email))
            {
                throw new DuplicateEntityFoundException($"Email is already registered.");
            }

            var userResult = await this.userManager.CreateAsync(appUser, randomPassword);

            if (userResult.Succeeded)
            {
                await this.userManager.AddToRoleAsync(appUser, "Employee");
                await this.applicationDbContext.SaveChangesAsync();
            }

            return userResult;
        }

        public async Task<AppUser> GetUserAsync(ClaimsPrincipal user)
        {
            return await this.userManager.Users
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
            return await this.userManager.FindByEmailAsync(email)
                ?? throw new Exception("User not found");
        }

        public async Task<bool> UserWithEmailExists(string email)
        {
            return await this.userManager.FindByEmailAsync(email) != null;
        }

        public async Task<AppUser> Update(AppUser appUser)
        {
            _ = await this.userManager.UpdateAsync(appUser);

            var updatedUser = await this.userManager.FindByIdAsync(appUser.Id)
                ?? throw new EntityNotFoundException("User not found.");

            return updatedUser;
        }
        public async Task<bool> Delete(AppUser user)
        {
            _ = await this.userManager.DeleteAsync(user)
                ?? throw new Exception("User not found");

            return true;
        }

        public async Task ResetPassword(AppUser user, string resetLink)
        {
            if (string.IsNullOrEmpty(resetLink))
            {
                throw new ArgumentNullException($"Password reset link in invalid");
            }

            var wwwrootPath = this.webHostEnvironment.WebRootPath;
            var filePath = Path.Combine(wwwrootPath, "PasswordResetConfirmation.html")
                ?? throw new EntityNotFoundException("Reset password email template not found.");

            string body;
            const string subject = "Password reset requested!";

            using (var reader = new StreamReader(filePath))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("{ResetLink}", resetLink);

            await this.emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task<IdentityResult> UpdateResetPassword(AppUser user, string resetToken, string newPassword, CancellationToken cancellationToken)
        {
            var result = await this.userManager.ResetPasswordAsync(user, resetToken, newPassword);

            if (result.Succeeded)
            {
                this.applicationDbContext.SaveChanges();
            }

            return result;
        }
    }
}
