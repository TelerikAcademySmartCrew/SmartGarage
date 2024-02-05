using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Areas.Admin.Controllers
{
    //[Authorize]
    public class HomeController : BaseAdminController
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAllEmployees()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterViewModel registertionData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Email", "Error. Please try again.");
                    return View();
                }

                var newUser = new AppUser
                {
                    UserName = registertionData.Email,
                    Email = registertionData.Email,
                    EmailConfirmed = true,
                    JoinDate = DateTime.UtcNow,
                };

                var result = await usersService.CreateEmployee(newUser);

                if (result.Succeeded)
                {
                    ViewData["PostRegisterMessage"] = "Registration successful! Please check your email";

                    return View("Index");
                }

                return View("Error");
            }
            catch (DuplicateEntityFoundException ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
            }
            catch (EntityNotFoundException ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Username", ex.Message);
                return View();
            }
        }
    }
}
