using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Models
{
    public class RegisterViewModel
    {
        // TODO : add validation
        [Required]
        public string? Email { get; set; } = null;
    }
}
