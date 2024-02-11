namespace SmartGarage.Common.Models.ViewModels
{
    public class AdminManageRepairActivityTypes
    {
        public ICollection<RepairActivityTypeViewModel> RepairActivities { get; set; } = new List<RepairActivityTypeViewModel>();

        public RepairActivityTypeViewModel RepairActivityRegister { get; set; } = new();
    }
}
