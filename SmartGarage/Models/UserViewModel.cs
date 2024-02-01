using SmartGarage.Data.Models;
using SmartGarage.Data.Models;

namespace SmartGarage.Models
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public IList<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public IList<Visit> Visits { get; set; } = new List<Visit>();
    }
}
