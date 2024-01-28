using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
