namespace SmartGarage.Common.Models.ViewModels
{
    public class GarageInfoViewModel
    {
        public UserViewModel userViewModel { get; set; } = new UserViewModel();
        public ICollection<RepairActivityTypeViewModel> RepairActivityTypes { get; set; } = new List<RepairActivityTypeViewModel>();
        public ICollection<VehicleBrandsAndModelsViewModel> VehicleBrandAndModels { get; set; } = new List<VehicleBrandsAndModelsViewModel>();
    }
}
