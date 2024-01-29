using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
