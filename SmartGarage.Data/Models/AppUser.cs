using Microsoft.AspNetCore.Identity;

namespace SmartGarage.Data.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime JoinDate { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}