using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class CustomersController : BaseEmployeeController
    {
        private readonly IUsersService usersService;
        private readonly IUserMapper userMapper;

        public CustomersController(IUsersService usersService, IUserMapper userMapper)
        {
            this.usersService = usersService;
            this.userMapper = userMapper;
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
            if (!this.ModelState.IsValid)
            {
                this.ModelState.AddModelError("Email", "Invalid email.");
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

                _ = await this.usersService.CreateUser(newCustomer);

                this.ViewData["CustomerRegisteredMessage"] = "Customer Registered";

                return View(registertionData);
            }
            catch (DuplicateEntityFoundException ex)
            {
                this.ModelState.AddModelError("Email", ex.Message);
                return View(registertionData);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageCustomers()
        {
            var allCustomers = await this.usersService.GetUsersInRoleAsync("Customer");

            var customersModel = this.userMapper.Map(allCustomers);

            base.InitializeUserName();

            return View(customersModel);
        }
    }
}
