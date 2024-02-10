using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using static SmartGarage.Common.GeneralApplicationConstants.Admin;

namespace SmartGarage.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : BaseAdminController
    {
        private readonly IUsersService usersService;
        private readonly IRepairActivityTypeService repairActivityTypeService;

        public HomeController(IUsersService usersService, IRepairActivityTypeService repairActivityTypeService)
        {
            this.usersService = usersService;
            this.repairActivityTypeService = repairActivityTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAllEmployees()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterViewModel registertionData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Email", "Error. Please try again.");
                    return View();
                }

                var newUser = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await usersService.CreateEmployee(newUser);

                if (result.Succeeded)
                {
                    ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";

                    return View("Index");
                }

                return View("Error");
            }
            catch (DuplicateEntityFoundException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View(registertionData);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateActivityType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivityType(RepairActivityTypeViewModel repairActivityViewModel)
        {
            try
            {
                var repairActivityType = new RepairActivityType
                {
                    Id = Guid.NewGuid(),
                    Name = repairActivityViewModel.Name
                };

                var newRepairActivityType = await this.repairActivityTypeService.CreateAsync(repairActivityType);

                return View("Index", "Home");
            }
            catch (EntityAlreadyExistsException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(repairActivityViewModel);
            }
            catch (DuplicateEntityFoundException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(repairActivityViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(repairActivityViewModel);
            }
        }
    }
}
