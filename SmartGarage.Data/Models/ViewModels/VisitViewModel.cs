namespace SmartGarage.Data.Models.ViewModels
{
    public class VisitViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public List<VisitRepairActivityViewModel> RepairActivities { get; set; } = new List<VisitRepairActivityViewModel>();
    }
}
