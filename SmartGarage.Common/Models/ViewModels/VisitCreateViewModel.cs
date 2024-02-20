using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.ViewModels
{
    public class VisitCreateViewModel
    {
        [MaxLength(17)]
        public string? VIN {  get; set; }

        [MaxLength(16)]
        public string? LicensePlateNumber {  get; set; }
    }
}
