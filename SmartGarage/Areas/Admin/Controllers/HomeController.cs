using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using static SmartGarage.Common.GeneralApplicationConstants.Admin;

namespace SmartGarage.Areas.Admin.Controllers
{
    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]
    public class HomeController : BaseAdminController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<AppUser> userManager;
        private readonly IUserMapper userMapper;
        private readonly IRepairActivityTypeService repairActivityTypeService;

        public HomeController(IUsersService usersService,
            UserManager<AppUser> userManager,
            IUserMapper userMapper,
            IRepairActivityTypeService repairActivityTypeService)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.userMapper = userMapper;
            this.repairActivityTypeService = repairActivityTypeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAllEmployees()
        {
            try
            {
                var employees = await this.userManager.GetUsersInRoleAsync("Employee");

                var employeesViewModel = this.userMapper.Map(employees);

                var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel
                {
                    Employees = employeesViewModel
                };
                
                return View(adminManageEmployeesViewModel);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel();

            return View(adminManageEmployeesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(
            RegisterEmployeeViewModel registerData)
        {
            var employees = await this.userManager.GetUsersInRoleAsync("Employee");
            var employeesVM = this.userMapper.Map(employees);
            var adminManageEmployeesViewModel = new AdminManageEmployeesViewModel
            {
                Employees = employeesVM
            };

            try
            {
                if (!this.ModelState.IsValid)
                {
                    this.ModelState.AddModelError("EmployeeRegisterData.Email", "Error. Please try again.");
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

                var result = await this.usersService.CreateEmployee(newUser);

                if (!result.Succeeded)
                {
                    this.ModelState.AddModelError("EmployeeRegisterData.Email", "Error. Please try again.");
                    return View("ViewAllEmployees", adminManageEmployeesViewModel);
                }

                adminManageEmployeesViewModel.Employees.Add(this.userMapper.Map(newUser));

                this.ViewData["PostRegisterMessage"] = "Registration successful!";
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (DuplicateEntityFoundException ex)
            {
                this.ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                this.ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("EmployeeRegisterData.Email", ex.Message);
                return View("ViewAllEmployees", adminManageEmployeesViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(string userName)
        {
            try
            {
                var employee = await this.usersService.GetByEmail(userName);

                _ = await this.usersService.Delete(employee);

                return RedirectToAction("ViewAllEmployees");
            }
            catch (EntityNotFoundException)
            {
                return RedirectToAction("ViewAllEmployees");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateActivityType()
        {
            var adminManageRepairActivityTypes = new AdminManageRepairActivityTypes();

            var allActivityTypes = await this.repairActivityTypeService.GetAllAsync();

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

            var allActivityTypes = await this.repairActivityTypeService.GetAllAsync();

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

                this.ViewData["PostCreateMessage"] = "Crete activity type successful!";
                return View(adminManageRepairActivityTypes);
            }
            catch (EntityAlreadyExistsException ex)
            {
                this.ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
            catch (DuplicateEntityFoundException ex)
            {
                this.ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("RepairActivityRegister.Name", ex.Message);
                return View(adminManageRepairActivityTypes);
            }
        }
    }
}
