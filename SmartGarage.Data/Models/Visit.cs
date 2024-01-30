using SmartGarage.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class Visit
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime Date { get; set; } = DateTime.UtcNow;

		[Required]
		public string UserId { get; set; } = null!;

		[ForeignKey(nameof(UserId))]
		public AppUser User { get; set; } = null!;

		[Required]
		public int VehicleId { get; set; }

		[ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; } = null!;
	}
}
