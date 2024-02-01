using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models
{
	public class RepairActivityType
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; } 

        public ICollection<RepairActivity> RepairActivities { get; set; } = new List<RepairActivity>();
    }
}
