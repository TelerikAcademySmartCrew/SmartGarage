namespace SmartGarage.Common.Models.ViewModels
{
    public class AdminManageEmployeesViewModel
    {
        public ICollection<UserViewModel> Employees { get; set; } = new List<UserViewModel>();
        public RegisterViewModel RegisterData { get; set; } = new();
    }
}
