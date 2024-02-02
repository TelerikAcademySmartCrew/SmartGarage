using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Contracts;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    [Authorize(Policy = "EmployeeRequired")]
    public class VehicleAPIController : ControllerBase
    {
        private readonly IVehicleService vehicleService;

        public VehicleAPIController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] VehicleQueryParameters vehicleQueryParameters)
        {
            try
            {
                var vehicles = await vehicleService.GetAllAsync(vehicleQueryParameters);
                return Ok(vehicles);
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
                return Ok(vehicles);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/vehicles/id
        [HttpGet("{vehicleId:int}")]
        public async Task<IActionResult> GetByIdAsync(int vehicleId)
        {
            try
            {
                var vehicle = await vehicleService.GetVehicleByIdAsync(vehicleId);
                return Ok(vehicle);
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
                var createdVehicle = await vehicleService.CreateVehicleAsync(vehicleCreateDto, customerEmail, cancellationToken);
                return CreatedAtAction("GetById",  new { vehicleId = createdVehicle.CreationYear}, createdVehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/vehicles/id
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVehicleAsync([FromRoute]int id, [FromBody] VehicleCreateDTO vehicleDto)
        {
            try
            {
                var updatedVehicle = await vehicleService.UpdateVehicleAsync(id, vehicleDto);
                return Ok(updatedVehicle);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/vehicles/delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVehicleAsync([FromRoute]int id)
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
}
