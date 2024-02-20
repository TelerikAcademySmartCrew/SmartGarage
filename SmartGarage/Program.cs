using Azure.Communication.Email;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Data.Repositories;
using SmartGarage.Data.Seeding;
using SmartGarage.Hubs;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Mappers;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;
using SmartGarage.Utilities.Contract;

namespace SmartGarage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Configure Emails
            var emailConnectionString = builder.Configuration.GetConnectionString("EmailConnectionString");
            builder.Services.AddScoped<IEmailService, AzureEmailService>();
            builder.Services.AddScoped<EmailClient>(_ => new EmailClient(emailConnectionString));

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
           .AddRoles<IdentityRole>()
           .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSignalR();

            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IVehicleMapper, VehicleMapper>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<IModelRepository, ModelRepository>();
            builder.Services.AddScoped<IModelService, ModelService>();
            builder.Services.AddScoped<IVisitService, VisitService>();
            builder.Services.AddScoped<IVisitRepository, VisitRepository>();
            builder.Services.AddScoped<IVisitMapper, VisitMapper>();
            builder.Services.AddScoped<IUserMapper, UserMapper>();
            builder.Services.AddScoped<IRepairActivityService, RepairActivityService>();
            builder.Services.AddScoped<IRepairActivityRepository, RepairActivityRepository>();
            builder.Services.AddScoped<IRepairActivityTypeService, RepairActivityTypeService>();
            builder.Services.AddScoped<IRepairActivityTypeRepository, RepairActivityTypeRepository>();
            builder.Services.AddScoped<IEnquiryService, EnquiryService>();
            builder.Services.AddScoped<IEnquiryRepository, EnquiryRepository>();
            builder.Services.AddScoped<IEnquiryModelMapper, EnquiryModelMapper>();

            // Configure Emails
            var emailConfig = builder.Configuration.GetSection("EmailConfig").Get<EmailConfig>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddScoped<AzureEmailService>();
            builder.Services.AddScoped<PasswordGenerator>();

            builder.Services.AddScoped<IUsersService, UsersService>();

            // Configure PDF Generator
            builder.Services.AddSingleton<PDFGenerator>();
            //builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                SeedData.Initialize(userManager, roleManager).Wait();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

            app.MapControllerRoute(
                name: "default",
                pattern: "/{controller=Auth}/{action=Login}/{id?}");
            
            app.MapRazorPages();

            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}