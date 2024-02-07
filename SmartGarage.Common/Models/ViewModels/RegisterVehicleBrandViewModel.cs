namespace SmartGarage.Common.Models.ViewModels
{
    public class RegisterVehicleBrandViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<RegisterVehicleModelViewModel> Models { get; set; } = new List<RegisterVehicleModelViewModel>();
    }
}
