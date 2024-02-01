using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SmartGarage.Data.Models;
using SmartGarage.Models;
using static SmartGarage.Common.GeneralApplicationConstants.Admin;

namespace SmartGarage.Controllers
{
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userService;

        public HomeController(ILogger<HomeController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this.userService = userManager;
        }

        public IActionResult Index()
        {
            if (this.User.IsInRole(AdminRoleName))
            {
                return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            }
           
            LocationLists model = new LocationLists();
            var locations = new List<Location>()
            {
                new Location(1, "SmartGarage", "SmartGarage", 42.65033853376936, 23.379256507391496)
            };
            model.Locations = locations;
            model.ServiceLocation = locations[0];
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
