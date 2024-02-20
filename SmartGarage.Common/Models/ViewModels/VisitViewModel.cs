namespace SmartGarage.Common.Models.ViewModels
{
    public class VisitViewModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        public string Rating { get; set; }

        public DateTime DateCreated { get; set; }

        public string UserName { get; set; }

        public string VehicleBrand { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleYear { get; set; }

        public double TotalPrice { get; set; }

        public double Discount { get; set; }

        public ICollection<VisitRepairActivityViewModel> RepairActivities { get; set; } = new List<VisitRepairActivityViewModel>();

        public ICollection<VisitRepairActivityCreateViewModel> RepairActivityTypes { get; set; } = new List<VisitRepairActivityCreateViewModel>();
    }
}
