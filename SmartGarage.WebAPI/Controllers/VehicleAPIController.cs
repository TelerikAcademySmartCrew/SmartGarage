using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;

namespace SmartGarage.WebAPI.Controllers;

[Route("api/vehicles")]
[ApiController]
[Authorize(Policy = "EmployeeRequired")]
public class VehicleAPIController : ControllerBase
{
    private readonly IVehicleService vehicleService;
    private readonly IVehicleDtoMapper vehicleDtoMapper;

    public VehicleAPIController(IVehicleService vehicleService,
        IVehicleDtoMapper vehicleDtoMapper)
    {
        this.vehicleService = vehicleService;
        this.vehicleDtoMapper = vehicleDtoMapper;
    }

    // GET: api/vehicles
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] VehicleQueryParameters vehicleQueryParameters)
    {
        try
        {
            var vehicles = await vehicleService.GetAllAsync(vehicleQueryParameters);
            var vehiclesToReturn = vehicleDtoMapper.Map(vehicles);
            return Ok(vehiclesToReturn);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/vehicles/users/id
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetVehiclesByUser([FromRoute]string userId, [FromQuery] VehicleQueryParameters vehicleQueryParameters)
    {
        try
        {
            var vehicles = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
            var vehiclesResponse = this.vehicleDtoMapper.Map(vehicles);
            return Ok(vehiclesResponse);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/vehicles/id
    [HttpGet("{vehicleId:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid vehicleId)
    {
        try
        {
            var vehicle = await vehicleService.GetVehicleByIdAsync(vehicleId);
            var vehicleResponse = this.vehicleDtoMapper.Map(vehicle);
            return Ok(vehicleResponse);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST: api/vehicles
    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] VehicleCreateDTO vehicleCreateDto, 
        [FromQuery] string customerEmail,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = vehicleDtoMapper.Map(vehicleCreateDto);
            var createdVehicle = await vehicleService.CreateVehicleAsync(vehicle, customerEmail, cancellationToken);
            var vehicleResponse = this.vehicleDtoMapper.Map(createdVehicle);
            return CreatedAtAction("GetById",  new { vehicleId = createdVehicle.Id}, vehicleResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/vehicles/id
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVehicleAsync([FromRoute]Guid id, [FromBody] VehicleCreateDTO vehicleDto)
    {
        try
        {
            var vehicle = vehicleDtoMapper.Map(vehicleDto);
            var updatedVehicle = await vehicleService.UpdateVehicleAsync(id, vehicle);
            return Ok(updatedVehicle);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/vehicles/delete
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteVehicleAsync([FromRoute]Guid id)
    {
        try
        {
            await vehicleService.DeleteVehicleAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}