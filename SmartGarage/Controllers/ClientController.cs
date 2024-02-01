using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Models;
using SmartGarage.Services.Services.Contracts;
using SmartGarage.Data.Models;

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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginData)
        {
            if (ModelState.IsValid)
            {
                var user = usersService.GetByEmail(loginData.Email);

                if (user != null)
                {
                    // Attempt to sign in the user
                    var result = await signInManager.PasswordSignInAsync(user.Result, loginData.Password, false, false);

                    // TODO : to check:
                    // IsEmailConfirmedAsync
                    // IsLockedOutAsync
                    // etc...

                    if (result.Succeeded)
                    {
                        // Successfully authenticated, redirect to the main page for logged-in users
                        return View(loginData); // Adjust this based on your controller and action
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            // If there are validation errors or login fails, redisplay the login form
            //return View(loginData);
            return NotFound("Not found");
        }

        public async Task<IActionResult> Index2(LoginViewModel loginViewModel)
        {
            try
            {
                var user = await usersService.GetByEmail(loginViewModel.Email);
                return View();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
