using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SmartGarage.Data.Models;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Data
{
	public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<RepairActivity> RepairActivities { get; set; }

        public DbSet<RepairActivityType> RepairActivityTypes { get; set; }

        public DbSet<VehicleBrand> VehicleBrands { get; set; }

        public DbSet<VehicleModel> VehicleModels { get; set; }

        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureEntityRelationships(builder);
        }

        private static void ConfigureEntityRelationships(ModelBuilder builder)
        {
            builder.Entity<Vehicle>()
                .HasOne(v => v.User)
                .WithMany(u => u.Vehicles)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Vehicle>()
                .HasOne(v => v.Brand)
                .WithMany(b => b.Vehicles)
                .HasForeignKey(v => v.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Visit>()
                .HasOne(v => v.Vehicle)
                .WithMany(v => v.Visits)
                .HasForeignKey(v => v.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<VehicleModel>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.Models)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepairActivity>()
                .HasOne(ra => ra.Visit)                
                .WithMany(v => v.RepairActivities)
                .HasForeignKey(ra => ra.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepairActivity>()
                .HasOne(s => s.RepairActivityType)
                .WithMany(s =>  s.RepairActivities)
                .HasForeignKey(s => s.RepairActivityTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
