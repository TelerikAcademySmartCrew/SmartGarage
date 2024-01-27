using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Models.VehicleModels
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public List<Model> Models { get; set; } = new List<Model>();
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
