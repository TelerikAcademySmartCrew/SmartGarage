using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class RepairActivity
    {
        [Key]
        public int Id { get; set; }

		[Required]
		public int RepairActivityTypeId { get; set; }

        [ForeignKey(nameof(RepairActivityTypeId))]
        public RepairActivityType RepairActivityType { get; set; } = null!;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

		[Required]
		public int VisitId { get; set; }

        [ForeignKey(nameof(VisitId))]
        public Visit Visit { get; set; } = null!;        
    }
}