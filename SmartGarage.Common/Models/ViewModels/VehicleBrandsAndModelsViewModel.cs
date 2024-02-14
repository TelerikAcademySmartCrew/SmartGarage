namespace SmartGarage.Common.Models.ViewModels
{
    public class VehicleBrandsAndModelsViewModel
    {
        public string Name { get; set; }
        public ICollection<string> Models { get; set; } = new List<string>();
    }
}
