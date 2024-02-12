using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Common.Models.ViewModels
{
    public class ManageVehiclesViewModel
    {
        public ICollection<VehicleViewModel> Vehicles { get; set; } = new List<VehicleViewModel>();
        public VehicleQueryParameters VehicleQueryParameters {  get; set; } = new ();
    }
}
