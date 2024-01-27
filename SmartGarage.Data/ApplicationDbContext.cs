using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartGarage.WebAPI.Models;
using System.Reflection.Emit;

namespace SmartGarage.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<AppUser> users = new List<AppUser>()
            {
                new AppUser()
                {
                    Id = new Guid().ToString(),
                }
            };

            builder.Entity<Vehicle>().HasData(users);

            //builder.Entity<Vehicle>()
            //    .WithMany(p => p.Vehicles)
            //    .HasForeignKey(r => r.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);

            List<Vehicle> vehicles = new List<Vehicle>()
            {
                new Vehicle()
                {
                    Id = new Guid().ToString(),
                    BrandId = "BMW",
                    UserId = users[0].Id
                }
            };

            builder.Entity<Vehicle>().HasData(users);

            builder.Entity<Vehicle>()
                .HasOne(l => l.User)
                .WithMany(p => p.Vehicles)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
