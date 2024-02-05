using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.ViewModels
{
    public class RegisterViewModel
    {
        // TODO : add validation
        [Required]
        public string? Email { get; set; } = null;
    }
}
