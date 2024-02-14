namespace SmartGarage.Common.Models.ViewModels
{
    public class AdminManageEmployeesViewModel
    {
        public ICollection<UserViewModel> Employees { get; set; } = new List<UserViewModel>();
        public RegisterEmployeeViewModel RegisterData { get; set; } = new();
    }
}
