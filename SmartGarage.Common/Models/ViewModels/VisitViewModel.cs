namespace SmartGarage.Common.Models.ViewModels;

public class VisitViewModel
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public string UserName { get; set; }
    public string VehicleBrand { get; set; }
    public string VehicleModel { get; set; }
    public double TotalPrice { get; set; }
    public List<VisitRepairActivityViewModel> RepairActivities { get; set; } = new ();
    public List<VisitRepairActivityCreateViewModel> RepairActivityTypes { get; set; } = new ();
}
