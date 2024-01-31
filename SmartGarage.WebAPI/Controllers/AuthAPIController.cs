using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/auth")]
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
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            AppUser user = new()
            {
                Email = userRegisterDto.Username,
                UserName = userRegisterDto.Username,
            };
            const string password = "Paramore789Bear!";
            var createdUserResult = await this.userManager.CreateAsync(user, password);
            if (createdUserResult.Succeeded)
            {
                return Ok("User created successfully!");
            }

            return BadRequest("Operation unsuccessful!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromHeader] string credentials)
        {
            var splitCredentials = credentials.Split(":");
            var email = splitCredentials[0];
            var password = splitCredentials[1];

            var user = this.userManager.FindByEmailAsync(email).Result;

            if (!await this.userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest("Invalid credentials!");
            }

            var tokenString = this.jwtService.GenerateJsonWebToken(user);
            return Ok(new { Token = tokenString });
        }
    }
}