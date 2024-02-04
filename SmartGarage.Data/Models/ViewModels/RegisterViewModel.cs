using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models.ViewModels
{
    public class RegisterViewModel
    {
        // TODO : add validation
        [Required]
        public string? Email { get; set; } = null;
    }
}
