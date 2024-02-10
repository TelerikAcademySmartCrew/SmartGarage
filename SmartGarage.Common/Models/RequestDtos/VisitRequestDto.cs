using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos
{
    public  class VisitRequestDto
    {
        [Required]
        public string CustomerEmail { get; set; }

        [Required] 
        public string LicensePlateNumber { get; set; }

        public int Rating { get; set; } 

        public ICollection<RepairActivityRequestDto> RepairActivities { get; set; } = new List<RepairActivityRequestDto>();
    }
}