using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.WebAPI.Models;
using SmartGarage.WebAPI.Services;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService usersService;

        internal UsersController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Create()
        {
            AppUser appUser = new AppUser();

            var user = usersService.Create(appUser);

            return Ok();
        }
    }
}