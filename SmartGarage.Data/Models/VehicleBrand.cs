using System.ComponentModel.DataAnnotations;

using SmartGarage.WebAPI.Models;

namespace SmartGarage.Data.Models
{
	public class VehicleBrand
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<VehicleModel> Models { get; set; } = new List<VehicleModel>();

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}