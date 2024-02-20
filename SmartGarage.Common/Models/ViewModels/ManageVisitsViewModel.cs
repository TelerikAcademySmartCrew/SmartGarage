using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Common.Models.ViewModels
{
    public class ManageVisitsViewModel
    {
        public ICollection<VisitViewModel> Visits { get; set; } = new List<VisitViewModel>();

        public VisitsQueryParameters VisitsQueryParameters { get; set; } = new();
    }
}
