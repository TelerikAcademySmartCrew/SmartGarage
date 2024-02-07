using Microsoft.AspNetCore.Mvc;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class CustomersController : BaseEmployeeController
    {
        private readonly IVehicleService vehicleService;

        public CustomersController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            InitializeUserName();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(string customerEmail)
        {
            if (!ModelState.IsValid)
            {

            }

            InitializeUserName();

            await Task.Delay(1);
            return View();
        }

        [HttpGet]
        public IActionResult ManageCustomers()
        {
            InitializeUserName();
            return View();
        }

    }
}
