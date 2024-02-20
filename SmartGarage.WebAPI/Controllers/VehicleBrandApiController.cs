using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class VehicleBrandApiController : ControllerBase
    {
        private readonly IBrandService vehicleBrandService;
        private readonly IVehicleBrandMapper vehicleBrandMapper;

        public VehicleBrandApiController(
            IBrandService vehicleBrandService,
            IVehicleBrandMapper vehicleBrandMapper)
        {
            this.vehicleBrandService = vehicleBrandService;
            this.vehicleBrandMapper = vehicleBrandMapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleBrandRequestDto dto)
        {
            var brand = this.vehicleBrandMapper.Materialize(dto);
            var created = await this.vehicleBrandService.CreateAsync(brand);

            return Ok(created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await this.vehicleBrandService.GetAllAsync();
            return Ok(brands);
        }        

        [HttpGet]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                var brand = await this.vehicleBrandService.GetByNameAsync(name);
                return Ok(brand);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            try
            {
                var brand = await this.vehicleBrandService.GetByIdAsync(id);
                return Ok(brand);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
