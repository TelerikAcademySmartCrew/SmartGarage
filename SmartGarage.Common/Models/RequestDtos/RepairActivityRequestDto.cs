using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos
{
    public class RepairActivityRequestDto
    {
        [Required]
        public RepairActivityTypeRequestDto RepairActivityType { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
