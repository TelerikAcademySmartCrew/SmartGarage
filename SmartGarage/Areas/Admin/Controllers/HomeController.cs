using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public HomeController()
        {
        }

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
