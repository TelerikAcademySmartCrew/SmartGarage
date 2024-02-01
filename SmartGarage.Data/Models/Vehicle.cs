using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SmartGarage.Common.Attributes;
using SmartGarage.Data.Models;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Vehicle;

namespace SmartGarage.WebAPI.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

		[Required]
		public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public VehicleBrand Brand { get; set; } = null!;

        [Required]
        public int ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public VehicleModel Model { get; set; } = null!;

        [Required]
        [StringLength(17)]
        public string VIN { get; set; } = null!;

        [Required]
        [IsBefore(ErrorMessage = InvalidYear)]
        public int ProductionYear { get; set; }

        [Required]
        [MaxLength(8)]
        public string LicensePlateNumber { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
        
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}