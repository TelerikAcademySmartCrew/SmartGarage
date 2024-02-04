using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.ViewModels;
using SmartGarage.Services.Services.Contracts;

namespace SmartGarage.Controllers
{
    public class VisitsController : Controller
    {
        //private readonly HttpClient httpClient;
        private readonly IUsersService usersService;
        private readonly ApplicationDbContext applicationDbContext;

        public VisitsController(IUsersService usersService,
            ApplicationDbContext applicationDbContext)
        {
            // This is how to connect to the REST API
            //this.httpClient = httpClient;
            //this.httpClient.BaseAddress = new Uri("https://localhost/api/"); // Replace with your API base URL

            this.usersService = usersService;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAll()
        {
            UserViewModel model = new UserViewModel();

            var user = await usersService.GetUserAsync(User);

            model.UserName = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.PhoneNumber = user.PhoneNumber;

            model.Vehicles = user.Vehicles.Select(vehicle => new VehicleViewModel
            {
                Brand = vehicle.Brand.Name,
                Model = vehicle.Model.Name,
                ProductionYear = vehicle.ProductionYear,
                VIN = vehicle.VIN,
                LicensePlateNumber = vehicle.LicensePlateNumber,
            }).ToList();

            model.Visits = user.Visits.Select(visit => new VisitViewModel
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
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult DisplayVisit(int visitId)
        {
            try
            {
                var visit = applicationDbContext.Visits
                    .Include(v => v.Vehicle)
                        .ThenInclude(v => v.Brand)
                    .Include(v => v.Vehicle)
                        .ThenInclude(v => v.Model)
                    .Include(v => v.RepairActivities)
                        .ThenInclude(v => v.RepairActivityType)
                    .Include(v => v.User)
                    .FirstOrDefault(x => x.Id == visitId)
                    ?? throw new EntityNotFoundException("Vist not found");

                VisitViewModel visitViewModel = new VisitViewModel
                {
                    Id = visit.Id,
                    DateCreated = visit.Date,
                    UserName = visit.User.UserName,
                    VehicleBrand = visit.Vehicle.Brand.Name,
                    VehicleModel = visit.Vehicle.Model.Name,
                    RepairActivities = visit.RepairActivities.Select(a => new VisitRepairActivityViewModel
                    {
                        Name = a.RepairActivityType.Name,
                        Price = a.Price,
                    }).ToList(),
                };

                return View(visitViewModel);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
