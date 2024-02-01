using Microsoft.AspNetCore.Mvc;
using SmartGarage.Models;

namespace SmartGarage.Controllers
{
    public class VisitsController : Controller
    {


        public VisitsController()
        {

        }

        public IActionResult DisplayAll()
        {
            List<VisitViewModel> visits = new List<VisitViewModel>()
            {
                new VisitViewModel()
                {
                    DateCreated = DateTime.Now,
                    VehicleBrand = "BMW",
                    VehicleModel = "M3 E30",
                    VehicleServices = new List<VisitServiceViewModel>()
                    {
                        new VisitServiceViewModel()
                        {
                            Name = "Oil change",
                            Price = "20lv."
                        },
                        new VisitServiceViewModel()
                        {
                            Name = "Air filter change",
                            Price = "28lv."
                        }
                    },
                },
                new VisitViewModel()
                {
                    DateCreated = DateTime.Now,
                    VehicleBrand = "Nissan",
                    VehicleModel = "S15",
                    VehicleServices = new List<VisitServiceViewModel>()
                    {
                        new VisitServiceViewModel()
                        {
                            Name = "Oil change",
                            Price = "20lv."
                        },
                        new VisitServiceViewModel()
                        {
                            Name = "Air filter change",
                            Price = "28lv."
                        }
                    },
                },
            };

            return View(visits);
        }

        public IActionResult DisplayVisit()
        {
            return View();
        }
    }
}
