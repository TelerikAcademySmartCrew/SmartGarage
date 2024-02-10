using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models
{
    public class Enquiry
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public string DateCreated { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");
    }
}
