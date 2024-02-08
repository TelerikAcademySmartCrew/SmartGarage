using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class CustomersController : BaseEmployeeController
    {
        private readonly IUsersService usersService;
        private readonly IVehicleService vehicleService;

        public CustomersController(IUsersService usersService, IVehicleService vehicleService)
        {
            this.usersService = usersService;
            this.vehicleService = vehicleService;
        }

        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            var model = new RegisterViewModel();
            base.InitializeUserName();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(RegisterViewModel registertionData)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Email", "Invalid email.");
                return View(registertionData);
            }

            base.InitializeUserName();

            try
            {
                var newCustomer = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                _ = await usersService.CreateUser(newCustomer);

                this.ViewData["CustomerRegisteredMessage"] = "Customer Registered";

                return View(registertionData);
            }
            catch (DuplicateEntityFoundException ex)
            {
                //this.ViewData["CustomerRegisteredMessage"] = null;
                ModelState.AddModelError("Email", ex.Message);
                return View(registertionData);
            }
        }

        [HttpGet]
        public IActionResult ManageCustomers()
        {
            base.InitializeUserName();
            return View();
        }

    }
}
