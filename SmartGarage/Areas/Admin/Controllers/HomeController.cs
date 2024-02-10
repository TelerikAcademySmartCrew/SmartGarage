﻿using Microsoft.AspNetCore.Authorization;
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

                return View(employeesVM);
            }
            catch (Exception ex)
            {
                return View();
            }
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
