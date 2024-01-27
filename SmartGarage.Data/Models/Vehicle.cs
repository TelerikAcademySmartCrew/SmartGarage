using SmartGarage.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.WebAPI.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public VehicleBrand Brand { get; set; }

        [Required]
        public int ModelId { get; set; }
        [ForeignKey(nameof(ModelId))]

        public VehicleModel Model { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }
        public List<Service> Services { get; set; } = new List<Service>();

        [Required]
        public bool IsDeleted { get; set; }
    }
}
