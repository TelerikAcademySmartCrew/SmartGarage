using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Controllers
{
    public class CustomerController : BaseCustomerController
    {
        private readonly IUsersService usersService;
        private readonly UserManager<AppUser> userManager;
        private readonly IUserMapper userMapper;
        private readonly IBrandService brandService;
        private readonly IRepairActivityTypeService repairActivityTypeService;

        public CustomerController(IUsersService usersService,
            UserManager<AppUser> userManager,
            IUserMapper userMapper,
            IBrandService brandService,
            IRepairActivityTypeService repairActivityTypeService)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.userMapper = userMapper;
            this.brandService = brandService;
            this.repairActivityTypeService = repairActivityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> SmartGarageInfo()
        {
            try
            {
                var user = await this.usersService.GetUserAsync(this.User);

                var userModel = this.userMapper.Map(user);

                var activityServiceTypes = await this.repairActivityTypeService.GetAllAsync();

                GarageInfoViewModel model = new GarageInfoViewModel
                {
                    UserViewModel = userModel,
                    RepairActivityTypes = activityServiceTypes.Select(a => new RepairActivityTypeViewModel
                    {
                        Name = a.Name,
                    }).ToList()
                };

                var allBrands = await this.brandService.GetAllAsync();
                foreach (var brand in allBrands)
                {
                    model.VehicleBrandAndModels.Add(new VehicleBrandsAndModelsViewModel
                    {
                        Name = brand.Name,
                        Models = brand.Models.Select(model => model.Name).ToList(),
                    });
                }

                return View(model);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound("Not found");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await this.usersService.GetUserAsync(User);

                var model = new UserViewModel
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Vehicles = user.Vehicles.Select(vehicle => new VehicleViewModel
                    {
                        Brand = vehicle.Brand.Name,
                        Model = vehicle.Model.Name,
                        CreationYear = vehicle.ProductionYear,
                        VIN = vehicle.VIN,
                        LicensePlate = vehicle.LicensePlateNumber,
                    }).ToList(),
                    Visits = user.Visits.Select(visit => new VisitViewModel
                    {
                        Id = visit.Id,
                        DateCreated = visit.Date,
                        VehicleBrand = visit.Vehicle.Brand.Name,
                        VehicleModel = visit.Vehicle.Model.Name,
                        RepairActivities = visit.RepairActivities.Select(a => new VisitRepairActivityViewModel
                        {
                            Name = a.RepairActivityType.Name,
                            Price = a.Price,
                        }).ToList(),
                    }).ToList()
                };

                return View(model);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound("Not found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfile(UserViewModel viewModel)
        {
            try
            {
                var user = await this.usersService.GetByEmail(viewModel.UserName!);

                user.FirstName = viewModel.FirstName!;
                user.LastName = viewModel.LastName!;
                user.PhoneNumber = viewModel.PhoneNumber;

                await this.usersService.Update(user);

                return RedirectToAction("Profile", "Customer");
            }
            catch (EntityNotFoundException)
            {
                this.ModelState.AddModelError("Email", "Error occured. Try again.");
                return View("Profile", viewModel);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("Email", "Error occured. Try again.");
                return View("Profile", viewModel);
            }
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var changePasswordViewModel = new UserChangePasswordViewModel();
            changePasswordViewModel.UserName = this.User.Identity!.Name!;

            return View(changePasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePasswordViewModel userChangePasswordData)
        {
            if (!this.ModelState.IsValid)
            {
                return View(userChangePasswordData);
            }

            try
            {
                var user = await this.usersService.GetByEmail(userChangePasswordData.UserName);

                var passwordCheckResult = await this.userManager.CheckPasswordAsync(user, userChangePasswordData.OldPassword);
                if (!passwordCheckResult)
                {
                    ModelState.AddModelError("OldPassword", "Current password is incorrect.");
                    return View(userChangePasswordData);
                }

                var result = await this.userManager.ChangePasswordAsync(user, userChangePasswordData.OldPassword, userChangePasswordData.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                else
                {
                    this.ModelState.AddModelError("NewPassword", result.Errors.First().Description);
                    return View(userChangePasswordData);
                }
            }
            catch (EntityNotFoundException ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
                return View(userChangePasswordData);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("Username", ex.Message);
                return View(userChangePasswordData);
            }
        }
    }
}
