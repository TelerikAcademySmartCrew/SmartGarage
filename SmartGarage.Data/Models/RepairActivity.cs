using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class RepairActivity
    {
        [Key]
        public Guid Id { get; set; }

		[Required]
		public Guid RepairActivityTypeId { get; set; }

        [ForeignKey(nameof(RepairActivityTypeId))]
        public RepairActivityType RepairActivityType { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

		[Required]
		public Guid VisitId { get; set; }

        [ForeignKey(nameof(VisitId))]
        public Visit Visit { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}