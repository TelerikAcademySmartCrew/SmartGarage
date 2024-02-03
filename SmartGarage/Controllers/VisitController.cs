using System.Collections;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Models;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Controllers
{
    public class VisitController : Controller
    {
        private readonly IVisitService visitService;

        public VisitController(IVisitService visitService)
        {
            this.visitService = visitService;
        }
        
        public async Task<IActionResult> DisplayAll([FromQuery] string id)
        {
            var userVisit = await this.visitService
                .GetByUserIdAsync(id);
            
            var userVisitAsViewModel = userVisit.Select(v => new VisitViewModel
                {
                    DateCreated = v.Date,
                    VehicleBrand = v.Vehicle.Brand.Name,
                    VehicleModel = v.Vehicle.Model.Name,
                    RepairActivities = v.RepairActivities
                        .Select(ra => new VisitRepairActivityViewModel
                        {
                            Name = ra.RepairActivityType.Name,
                            Price = ra.Price
                        })
                        .ToList()
                })
                .ToList();
            
            
            // List<VisitViewModel> visits = new List<VisitViewModel>()
            // {
            //     new VisitViewModel()
            //     {
            //         DateCreated = DateTime.Now,
            //         VehicleBrand = "BMW",
            //         VehicleModel = "M3 E30",
            //         VehicleServices = new List<VisitServiceViewModel>()
            //         {
            //             new VisitServiceViewModel()
            //             {
            //                 Name = "Oil change",
            //                 Price = "20lv."
            //             },
            //             new VisitServiceViewModel()
            //             {
            //                 Name = "Air filter change",
            //                 Price = "28lv."
            //             }
            //         },
            //     },
            //     new VisitViewModel()
            //     {
            //         DateCreated = DateTime.Now,
            //         VehicleBrand = "Nissan",
            //         VehicleModel = "S15",
            //         VehicleServices = new List<VisitServiceViewModel>()
            //         {
            //             new VisitServiceViewModel()
            //             {
            //                 Name = "Oil change",
            //                 Price = "20lv."
            //             },
            //             new VisitServiceViewModel()
            //             {
            //                 Name = "Air filter change",
            //                 Price = "28lv."
            //             }
            //         },
            //     },
            // };

            return View(userVisitAsViewModel);
        }

        public IActionResult DisplayVisit()
        {
            return View();
        }
    }
}
