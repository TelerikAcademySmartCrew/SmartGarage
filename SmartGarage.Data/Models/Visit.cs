using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using SmartGarage.Common.Enumerations;

namespace SmartGarage.Data.Models
{
	public class Visit
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public DateTime Date { get; set; } = DateTime.UtcNow;

		[Required]
		public string UserId { get; set; } = null!;

		[ForeignKey(nameof(UserId))]
		public AppUser User { get; set; } = null!;

		[Required]
		public Guid VehicleId { get; set; }

		[ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; } = null!;

		[Required]
		public Status Status { get; set; } = Status.Pending;

        public int Rating { get; set; }
		
        public int DiscountPercentage { get; set; }

        public ICollection<RepairActivity> RepairActivities { get; set; } = new List<RepairActivity>();
	}
}
