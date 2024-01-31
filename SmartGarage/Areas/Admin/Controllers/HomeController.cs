using Microsoft.AspNetCore.Mvc;
using SmartGarage.Services.Services.Contracts;
using SmartGarage.WebAPI.Models;

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
    }
}
