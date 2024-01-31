using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services;
using SmartGarage.Utilities;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly JwtService jwtService;
        private readonly PasswordGenerator passwordGenerator;

        public AuthAPIController(UserManager<AppUser> userManager,
            JwtService jwtService,
            PasswordGenerator passwordGenerator)
        {
            this.userManager = userManager;
            this.jwtService = jwtService;
            this.passwordGenerator = passwordGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDto)
        {
            AppUser user = new()
            {
                Email = userRegisterDto.Username,
                UserName = userRegisterDto.Username,
            };
            // const string password = "Paramore789Bear!";
            var password = this.passwordGenerator.Generate();
            var createdUserResult = await this.userManager.CreateAsync(user, password);
            
            switch (userRegisterDto.Role)
            {
                case "Employee":
                    await userManager.AddToRoleAsync(user, "Employee");
                    break;
                case "Customer":
                    await userManager.AddToRoleAsync(user, "Customer");
                    break;
            }

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