using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Controllers
{
    public class CustomerController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly IUsersService usersService;
        private readonly IUserMapper userMapper;
        private readonly IVehicleMapper vehicleMapper;
        private readonly IBrandService brandService;
        private readonly IRepairActivityTypeService repairActivityTypeService;

        public CustomerController(SignInManager<AppUser> signInManager,
            IUsersService usersService,
            IUserMapper userMapper,
            IVehicleMapper vehicleMapper,
            IBrandService brandService,
            IRepairActivityTypeService repairActivityTypeService)
        {
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.userMapper = userMapper;
            this.vehicleMapper = vehicleMapper;
            this.brandService = brandService;
            this.repairActivityTypeService = repairActivityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> SmartGarageInfo()
        {
            // TODO : 
            //if (User.IsInRole("User"))
            try
            {
                GarageInfoViewModel model = new GarageInfoViewModel();
                var user = await usersService.GetUserAsync(User);

                var userModel = userMapper.Map(user);

                model.userViewModel = userModel;
                var activityServiceTypes = await repairActivityTypeService.GetAllAsync();
                model.RepairActivityTypes = activityServiceTypes.Select(a => new RepairActivityTypeViewModel
                {
                    Name = a.Name,
                }).ToList();

                var allBrands = await brandService.GetAllAsync();
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
                var user = await usersService.GetUserAsync(User);

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
                var user = await usersService.GetByEmail(viewModel.UserName);

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.PhoneNumber = viewModel.PhoneNumber;

                await usersService.Update(user);

                return RedirectToAction("Profile", "Customer");
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("Email", "Error occured. Try again.");
                return View("Profile", viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Email", "Error occured. Try again.");
                return View("Profile", viewModel);
            }
        }
    }
}
