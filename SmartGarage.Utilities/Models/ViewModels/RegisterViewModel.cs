using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Utilities.Models.ViewModels
{
    public class RegisterViewModel
    {
        // TODO : add validation
        [Required]
        public string? Email { get; set; } = null;
    }
}
