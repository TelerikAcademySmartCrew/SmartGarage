using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.WebAPI.Models
{
    public class Vehicle
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string BrandId { get; set; }

        [Required]
        public string ModelId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; }
    }
}
