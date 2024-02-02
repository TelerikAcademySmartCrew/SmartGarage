using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Data.Models;

namespace SmartGarage.WebAPI.Controllers;

[Route("api/users/register")]
[ApiController]
[Authorize]
public class UsersAPIController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;
    private readonly PasswordGenerator passwordGenerator;
    private readonly IUserMapper userMapper;

    public UsersAPIController(UserManager<AppUser> userManager,
        PasswordGenerator passwordGenerator,
        IUserMapper userMapper)
    {
        this.userManager = userManager;
        this.passwordGenerator = passwordGenerator;
        this.userMapper = userMapper;
    }

    [Authorize(Policy = "AdminRequired")]
    [HttpPost("employees")]
    public async Task<IActionResult> RegisterEmployee([FromBody] UserRegisterDTO userRegisterDto)
    {
        var user = this.userMapper.Map(userRegisterDto);
        var password = this.passwordGenerator.Generate();
        var createdUserResult = await this.userManager.CreateAsync(user, password);

        if (!createdUserResult.Succeeded) return BadRequest("Operation unsuccessful!");
        await userManager.AddToRoleAsync(user, "Employee");
        return Ok(password);
    }
    
    [Authorize(Policy = "EmployeeRequired")]
    [HttpPost("customers")]
    public async Task<IActionResult> RegisterCustomer([FromBody] UserRegisterDTO userRegisterDto)
    {
        var user = this.userMapper.Map(userRegisterDto);
        var password = this.passwordGenerator.Generate();
        var createdUserResult = await this.userManager.CreateAsync(user, password);

        if (!createdUserResult.Succeeded) return BadRequest("Operation unsuccessful!");
        await userManager.AddToRoleAsync(user, "Customer");
        return Ok("Customer created successfully!");
    }
}