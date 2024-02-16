using System.Security.Claims;
using Azure;
using Azure.Communication.Email;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Mappers;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly EmailService emailService;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PasswordGenerator passwordGenerator;
        private readonly EmailClient emailClient;
        private const string SenderEmail = "DoNotReply@e5b418ff-9ee5-4fdc-b08f-e8bcf7bfc02c.azurecomm.net";

        public UsersService(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext applicationDbContext,
            EmailService emailService,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment,
            PasswordGenerator passwordGenerator,
            EmailClient emailClient)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.applicationDbContext = applicationDbContext;
            this.emailService = emailService;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.passwordGenerator = passwordGenerator;
            this.emailClient = emailClient;
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
            // TODO : use the pass generator when ready

            // Generate a random password
            string randomPassword = passwordGenerator.Generate();

            // Get the wwwroot path
            var wwwrootPath = webHostEnvironment.WebRootPath;

            
            var filePath = Path.Combine(wwwrootPath, "Views/MailTemplate/AccountConfirmation.html")
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
            // await emailService.SendEmailAsync(appUser.Email, subject, body);
            await emailClient.SendAsync(WaitUntil.Completed, SenderEmail, appUser.Email, subject, body);

            return userResult;
        }

        public async Task<IdentityResult> CreateEmployee(AppUser appUser)
        {
            // TODO : use the pass generator when ready

            // Generate a random password
            string randomPassword = "@User123";
            //string randomPassword = GenerateRandomPassword2();

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
