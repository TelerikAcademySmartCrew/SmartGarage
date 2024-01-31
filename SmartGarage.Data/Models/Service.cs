using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class Service
    {
        [Key]
        public int Id { get; set; }

		[Required]
		public int ServiceTypeId { get; set; }

        [ForeignKey(nameof(ServiceTypeId))]
        public ServiceType ServiceType { get; set; } = null!;

        [Required]
        public double Price { get; set; }

		[Required]
		public int VisitId { get; set; }

        [ForeignKey(nameof(VisitId))]
        public Visit Visit { get; set; } = null!;
    }
}