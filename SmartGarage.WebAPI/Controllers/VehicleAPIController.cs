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
            var vehicles = await vehicleService.GetAllAsync(vehicleQueryParameters, cancellationToken);
            var result = vehicleMapper.ToViewModel(vehicles);
            return Ok(result);
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
            var vehicles = await vehicleService.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken);
            var result = this.vehicleMapper.ToViewModel(vehicles);
            return Ok(result);
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
            var vehicle = await vehicleService.GetVehicleByIdAsync(vehicleId, cancellationToken);
            var result = this.vehicleMapper.ToViewModel(vehicle);
            return Ok(result);
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
            var result = this.vehicleMapper.ToViewModel(createdVehicle);
            return CreatedAtAction("GetById",  new { vehicleId = createdVehicle.Id}, result);
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
            var updatedVehicle = await vehicleService.UpdateVehicleAsync(id, vehicle, cancellationToken);
            var result = this.vehicleMapper.ToViewModel(updatedVehicle);
            return Ok(result);
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
            await vehicleService.DeleteVehicleAsync(id, cancellationToken);
            return Ok();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}