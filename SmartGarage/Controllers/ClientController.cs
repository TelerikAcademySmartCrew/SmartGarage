using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;
using SmartGarage.Utilities.Models.ViewModels;
namespace SmartGarage.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUsersService usersService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IVehicleMapper vehicleMapper;

        public ClientController(IUsersService usersService, SignInManager<AppUser> signInManager, IVehicleMapper vehicleMapper)
        {
            this.usersService = usersService;
            this.signInManager = signInManager;
            this.vehicleMapper = vehicleMapper;
        }

        [HttpGet]
        public async Task<IActionResult> SmartGarageInfo()
        {
            //if (this.User.IsInRole("User"))
            //{
            //    return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            //}

            //LocationsList model = new LocationsList();
            //var locations = new List<Location>()
            //{
            //    new Location(1, "SmartGarage", "SmartGarage", 42.65033853376936, 23.379256507391496)
            //};
            //model.Locations = locations;
            //model.ServiceLocation = locations[0];

            // TODO : 
            //if (User.IsInRole("User"))
            try
            {
                GarageInfoViewModel model = new GarageInfoViewModel();
                var user = await usersService.GetUserAsync(User);

                var userModel = new UserViewModel()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                };

                model.userViewModel = userModel;

                return View(model);
            }
            catch (Exception ex)
            {
                // If there are validation errors or login fails, redisplay the login form
                //return View(loginData);
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

                return RedirectToAction("Profile", "Client");
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
