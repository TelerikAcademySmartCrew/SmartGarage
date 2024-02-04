using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
