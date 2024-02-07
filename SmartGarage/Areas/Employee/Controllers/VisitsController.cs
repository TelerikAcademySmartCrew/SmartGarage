using Microsoft.AspNetCore.Mvc;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class VisitsController : BaseEmployeeController
    {

        [HttpGet]
        public IActionResult ManageVisits()
        {
            InitializeUserName();
            return View();
        }


        [HttpGet]
        public IActionResult ShowActiveVisits()
        {
            InitializeUserName();
            return View();
        }
    }
}
