namespace SmartGarage.Common.Models.ViewModels
{
    public class UserViewModel
    {
        public string? UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public ICollection<VehicleViewModel> Vehicles { get; set; } = new List<VehicleViewModel>();
        public ICollection<VisitViewModel> Visits { get; set; } = new List<VisitViewModel>();
    }
}