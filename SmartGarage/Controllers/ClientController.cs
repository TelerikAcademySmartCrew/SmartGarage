using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Models;
using SmartGarage.Services.Services.Contracts;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Controllers
{
    public class ClientController : Controller
    {
        private readonly IUsersService usersService;
        private readonly SignInManager<AppUser> signInManager;

        public ClientController(IUsersService usersService, SignInManager<AppUser> signInManager)
        {
            this.usersService = usersService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SmartGarageInfo()
        {
            //if (this.User.IsInRole("User"))
            //{
            //    return this.RedirectToAction("Index", "Home", new { Area = AdminAreaName });
            //}

            LocationLists model = new LocationLists();
            var locations = new List<Location>()
            {
                new Location(1, "SmartGarage", "SmartGarage", 42.65033853376936, 23.379256507391496)
            };
            model.Locations = locations;
            model.ServiceLocation = locations[0];

            // TODO : 
            //if (User.IsInRole("User"))
            {
                return View();
            }

            // If there are validation errors or login fails, redisplay the login form
            //return View(loginData);
            return NotFound("Not found");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var user = await usersService.GetUserAsync(User);

                var model = new UserViewModel()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                };

                return View(model);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound("Not found");
            }
        }
    }
}
