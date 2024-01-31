using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Contracts;

namespace SmartGarage.WebAPI.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var vehicle = await vehicleService.GetVehicleByIdAsync(id);
                return Ok(vehicle);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

       // POST: api/vehicles
       [HttpPost]
        public async Task<IActionResult> CreateVehicleAsync([FromBody] VehicleCreateDTO vehicleCreateDTO)
        {
            try
            {
                string userId = "629ddf3d-addf-45df-837a-c5ce82e8ba73";
                var createdVehicle = await vehicleService.CreateVehicleAsync(vehicleCreateDTO, userId);
                return Ok(createdVehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/vehicles/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleAsync([FromRoute]int id, [FromBody] VehicleCreateDTO vehicleDTO)
        {
            try
            {
                var vehicle = await vehicleService.GetVehicleByIdAsync(id);
                var updatedVehicle = await vehicleService.UpdateVehicleAsync(id, vehicleDTO);
                return Ok(updatedVehicle);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/vehicles/delete
        [HttpDelete("{id}")]
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
