using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Utilities.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
