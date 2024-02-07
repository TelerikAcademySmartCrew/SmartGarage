using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos
{
    public class RepairActivityTypeRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
