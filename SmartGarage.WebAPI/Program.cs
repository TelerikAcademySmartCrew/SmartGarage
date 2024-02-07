using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SmartGarage.Data;
using SmartGarage.Data.Repositories;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Data.Models;
using SmartGarage.Data.Seeding;
using SmartGarage.Utilities.Mappers;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            
            // JWT Configuration
            // Configure JWT authentication for swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartGarageAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
            
            var key = Encoding.ASCII.GetBytes("my-super-secret-key");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRequired", policy =>
                {
                    policy.RequireRole("Admin");
                });
                options.AddPolicy("EmployeeRequired", policy =>
                    policy.RequireRole("Employee"));
            });
            // Configure scope

            builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
          .AddRoles<IdentityRole>()
          .AddRoleManager<RoleManager<IdentityRole>>()
               .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();

            builder.Services.AddScoped<IVisitRepository, VisitRepository>();
            builder.Services.AddScoped<IVisitService, VisitService>();

            builder.Services.AddScoped<IRepairActivityRepository, RepairActivityRepository>();
            builder.Services.AddScoped<IRepairActivityService, RepairActivityService>();

            builder.Services.AddScoped<IRepairActivityTypeRepository, RepairActivityTypeRepository>();
            builder.Services.AddScoped<IRepairActivityTypeService, RepairActivityTypeService>();

            builder.Services.AddScoped<IVehicleMapper, VehicleMapper>();
            builder.Services.AddScoped<IVisitMapper, VisitMapper>();
            builder.Services.AddScoped<IUserMapper, UserMapper>();
            builder.Services.AddScoped<PasswordGenerator>();

            
            var jwtSecret = configuration["JwtSettings:Secret"];
            builder.Services.AddScoped<JwtService>(_ => new JwtService(
                _.GetRequiredService<IConfiguration>(),
                _.GetRequiredService<UserManager<AppUser>>(),
                jwtSecret));

            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
