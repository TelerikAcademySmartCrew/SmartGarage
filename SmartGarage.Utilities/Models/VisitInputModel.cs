using System.ComponentModel.DataAnnotations;
using SmartGarage.Data.Models;

namespace SmartGarage.Utilities.Models
{
    public class VisitInputModel
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public Guid VisitId { get; set; }
    }
}

