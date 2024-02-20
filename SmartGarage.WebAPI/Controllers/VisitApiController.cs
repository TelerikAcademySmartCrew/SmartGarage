using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.User;

namespace SmartGarage.WebAPI.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitApiController : ControllerBase
{
    private readonly IVisitService visitService;
    private readonly IVisitMapper visitMapper;
    private readonly UserManager<AppUser> userManager;
    private readonly IVehicleService vehicleService;

    public VisitApiController(
        IVisitService visitService,
        IVisitMapper visitMapper,
        UserManager<AppUser> userManager,
        IVehicleService vehicleService
        )
    {
        this.visitService = visitService;
        this.visitMapper = visitMapper;
        this.userManager = userManager;
        this.vehicleService = vehicleService;
    }

    [HttpGet("users/userId")]
    public async Task<IActionResult> GetUserVisits([FromRoute] string userId, CancellationToken cancellationToken)
    {
        var visits = await this.visitService.GetByUserIdAsync(userId, cancellationToken);
        var viewModel = this.visitMapper.ToViewModel(visits);

        if (viewModel.Any())
        {
            return Ok(viewModel);
        }

        return NoContent();
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetVisitById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var visit = await this.visitService.GetByIdAsync(id, cancellationToken);
            var viewModel = this.visitMapper.ToViewModel(visit);

            return Ok(viewModel);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VisitRequestDto visitRequestDto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await this.userManager.FindByEmailAsync(visitRequestDto.CustomerEmail)
                ?? throw new EntityNotFoundException(UserNotFound);

            var vehicle = await this.vehicleService.GetVehicleByLicensePlateAsync(visitRequestDto.LicensePlateNumber, cancellationToken);
            var visit = this.visitMapper.MaterializeRequestDto(visitRequestDto, user.Id, vehicle.Id);            

            await this.visitService.CreateAsync(visit, vehicle.Visits.Count, cancellationToken);
            
            return Ok(visit);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}