using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Controllers
{
    public class HomeController : Controller
    {
        // NOTE : to refer to when need to show the map
        //if (this.User.IsInRole(AdminRoleName))
        //{
        //    return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
        //}
        //LocationLists model = new LocationLists();
        //var locations = new List<Location>()
        //{
        //    new Location(1, "SmartGarage", "SmartGarage", 42.65033853376936, 23.379256507391496)
        //};
        //model.Locations = locations;
        //model.ServiceLocation = locations[0];


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
