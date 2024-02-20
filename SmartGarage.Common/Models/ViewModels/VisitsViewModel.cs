namespace SmartGarage.Common.Models.ViewModels
{
    public class VisitsViewModel
    {
        public List<VisitViewModel> Visits { get; set; } = new List<VisitViewModel>();

        public UserViewModel User { get; set; } = new UserViewModel();
    }
}
