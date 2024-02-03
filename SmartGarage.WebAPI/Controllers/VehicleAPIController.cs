using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

namespace SmartGarage.WebAPI.Controllers;

[Route("api/vehicles")]
[ApiController]
[Authorize(Policy = "EmployeeRequired")]
public class VehicleAPIController : ControllerBase
{
    private readonly IVehicleService vehicleService;
    private readonly IVehicleMapper vehicleMapper;

    public VehicleAPIController(IVehicleService vehicleService,
        IVehicleMapper vehicleMapper)
    {
        this.vehicleService = vehicleService;
        this.vehicleMapper = vehicleMapper;
    }

    // GET: api/vehicles
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] VehicleQueryParameters vehicleQueryParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicles = await vehicleService.GetAllAsync(vehicleQueryParameters);
            var vehiclesToReturn = vehicleMapper.ToViewModel(vehicles);
            return Ok(vehiclesToReturn);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/vehicles/users/id
    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetVehiclesByUser([FromRoute]string userId,
        [FromQuery] VehicleQueryParameters vehicleQueryParameters,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicles = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
            var vehiclesResponse = this.vehicleMapper.ToViewModel(vehicles);
            return Ok(vehiclesResponse);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // GET: api/vehicles/id
    [HttpGet("{vehicleId:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await vehicleService.GetVehicleByIdAsync(vehicleId);
            var vehicleResponse = this.vehicleMapper.ToViewModel(vehicle);
            return Ok(vehicleResponse);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST: api/vehicles
    [HttpPost]
    public async Task<IActionResult> CreateVehicleAsync([FromBody] VehicleInputModel vehicleInputModel, 
        [FromQuery] string customerEmail,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = vehicleMapper.MaterializeInputModel(vehicleInputModel);
            var createdVehicle = await vehicleService.CreateVehicleAsync(vehicle, customerEmail, cancellationToken);
            var vehicleResponse = this.vehicleMapper.ToViewModel(createdVehicle);
            return CreatedAtAction("GetById",  new { vehicleId = createdVehicle.Id}, vehicleResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/vehicles/id
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVehicleAsync([FromRoute]Guid id, 
        [FromBody] VehicleInputModel vehicleDto, 
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = vehicleMapper.MaterializeInputModel(vehicleDto);
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
    public async Task<IActionResult> DeleteVehicleAsync([FromRoute]Guid id,
        CancellationToken cancellationToken)
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