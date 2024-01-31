using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services;
using SmartGarage.Services.Services.Contracts;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly JwtService jwtService;

        public AuthAPIController(UserManager<AppUser> userManager,
            JwtService jwtService)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] string email)
        {
            AppUser user = new()
            {
                Email = email,
            };
            await this.userManager.CreateAsync(user);
            return Ok("User created successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] string credentials)
        {
            var splitCredentials = credentials.Split(":");
            var username = splitCredentials[0];
            var password = splitCredentials[1];

            var user = await this.userManager.FindByNameAsync(username);
            if (!await this.userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest("Invalid credentials!");
            }

            var tokenString = this.jwtService.GenerateJsonWebToken(user);
            return Ok(tokenString);
        }
    }
}
