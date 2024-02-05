using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.InputModels
{
    public class VisitInputModel
    {
        [Required]
        public Guid CustomerId { get; set; }
        
        [Required]
        public Guid VisitId { get; set; }
    }
}

