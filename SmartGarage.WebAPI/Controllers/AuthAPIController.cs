using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services;
using SmartGarage.Utilities;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

namespace SmartGarage.WebAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;
    private readonly JwtService jwtService;
    private readonly PasswordGenerator passwordGenerator;
    private readonly IUserMapper userMapper;

    public AuthAPIController(UserManager<AppUser> userManager,
        JwtService jwtService,
        PasswordGenerator passwordGenerator,
        IUserMapper userMapper)
    {
        this.userManager = userManager;
        this.jwtService = jwtService;
        this.passwordGenerator = passwordGenerator;
        this.userMapper = userMapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterInputModel userRegisterInputModel)
    {
        var user = this.userMapper.MaterializeInputModel(userRegisterInputModel);
        var password = this.passwordGenerator.Generate();
        var createdUserResult = await this.userManager.CreateAsync(user, password);

        if (!createdUserResult.Succeeded) return BadRequest("Operation unsuccessful!");
        await userManager.AddToRoleAsync(user, "Customer");
        return Ok(password);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromHeader] string credentials)
    {
        var splitCredentials = credentials.Split(":");
        var email = splitCredentials[0];
        var password = splitCredentials[1];

        var user = await this.userManager.FindByEmailAsync(email);

        if (!await this.userManager.CheckPasswordAsync(user, password))
        {
            return BadRequest("Invalid credentials!");
        }

        var tokenString = this.jwtService.GenerateJsonWebToken(user);
        return Ok(new { Token = tokenString });
    }
}