using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.ViewModels
{
    public class RepairActivityTypeViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
