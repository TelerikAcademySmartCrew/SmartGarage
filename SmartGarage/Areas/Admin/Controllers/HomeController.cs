using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers;
using SmartGarage.Utilities.Mappers.Contracts;
using static SmartGarage.Common.GeneralApplicationConstants.Admin;

namespace SmartGarage.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : BaseAdminController
    {
        private readonly IUsersService usersService;
        private readonly IRepairActivityTypeService repairActivityTypeService;
        private readonly UserManager<AppUser> userManager;
        private readonly IUserMapper userMapper;

        public HomeController(IUsersService usersService,
            IRepairActivityTypeService repairActivityTypeService,
            UserManager<AppUser> userManager,
            IUserMapper userMapper)
        {
            this.usersService = usersService;
            this.repairActivityTypeService = repairActivityTypeService;
            this.userManager = userManager;
            this.userMapper = userMapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllEmployees()
        {
            try
            {
                var employees = await userManager.GetUsersInRoleAsync("Employee");

                var employeesVM = userMapper.Map(employees);

                var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel();
                adminManageEmployeesViewModel.Employees = employeesVM;
                return View(adminManageEmployeesViewModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RegisterEmployee()
        {
            var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel();

            return View(adminManageEmployeesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(
            //[Bind(nameof(AdminManageEmployeesViewModel.RegisterData))]
            RegisterEmployeeViewModel registerData)
        {
            var employees = await userManager.GetUsersInRoleAsync("Employee");
            var employeesVM = userMapper.Map(employees);
            var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel();
            adminManageEmployeesViewModel.Employees = employeesVM;

            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("EmployeeRegisterData.Email", "Error. Please try again.");
                    return View("ViewAllEmployees", adminManageEmployeesViewModel);
                }

                var newUser = new AppUser
                {
                    UserName = registerData.Email,
                    Email = registerData.Email,
                    FirstName = registerData.FirstName,
                    LastName = registerData.LastName,
                    PhoneNumber = registerData.PhoneNumber,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await usersService.CreateEmployee(newUser);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("EmployeeRegisterData.Email", "Error. Please try again.");
                    return View("ViewAllEmployees", adminManageEmployeesViewModel);
                }

                adminManageEmployeesViewModel.Employees.Add(userMapper.Map(newUser));

                ViewData["PostRegisterMessage"] = "Registration successful!";
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (DuplicateEntityFoundException ex)
            {
                ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(string userName)
        {
            var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel();

            try
            {
                var employee = await usersService.GetByEmail(userName);
                _ = await usersService.Delete(employee);
                return RedirectToAction("ViewAllEmployees");
            }
            catch (EntityNotFoundException ex)
            {
                return RedirectToAction("ViewAllEmployees");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateActivityType()
        {
            var adminManageRepairActivityTypes = new AdminManageRepairActivityTypes();

            var allActivityTypes = await repairActivityTypeService.GetAllAsync();

            adminManageRepairActivityTypes.RepairActivities = allActivityTypes.Select(activity => new RepairActivityTypeViewModel
            {
                Name = activity.Name,
            }).ToList();

            return View(adminManageRepairActivityTypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivityType([Bind(nameof(RepairActivityTypeViewModel.Name))] RepairActivityTypeViewModel repairActivityName)
        {
            var adminManageRepairActivityTypes = new AdminManageRepairActivityTypes();
            var allActivityTypes = await repairActivityTypeService.GetAllAsync();
            adminManageRepairActivityTypes.RepairActivities = allActivityTypes.Select(activity => new RepairActivityTypeViewModel
            {
                Name = activity.Name,
            }).ToList();

            try
            {
                var repairActivityType = new RepairActivityType
                {
                    Id = Guid.NewGuid(),
                    Name = repairActivityName.Name
                };

                var newRepairActivityType = await this.repairActivityTypeService.CreateAsync(repairActivityType);

                adminManageRepairActivityTypes.RepairActivityRegister = new RepairActivityTypeViewModel
                {
                    Name = newRepairActivityType.Name
                };

                adminManageRepairActivityTypes.RepairActivities.Add(new RepairActivityTypeViewModel
                {
                    Name = newRepairActivityType.Name
                });

                ViewData["PostCreateMessage"] = "Crete activity type successful!";
                return View(adminManageRepairActivityTypes);
            }
            catch (EntityAlreadyExistsException ex)
            {
                ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
            catch (DuplicateEntityFoundException ex)
            {
                ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
        }
    }
}
