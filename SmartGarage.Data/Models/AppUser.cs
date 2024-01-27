using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models.VehicleModels;

namespace SmartGarage.WebAPI.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime JoinDate { get; set; }
        public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public IList<Service> Services { get; set; } = new List<Service>();
    }
}
