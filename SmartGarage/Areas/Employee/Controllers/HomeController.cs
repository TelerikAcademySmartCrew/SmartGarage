using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class HomeController : BaseEmployeeController
    {
        public IActionResult Index()
        {
            InitializeUserName();

            return View();
        }
    }
}
