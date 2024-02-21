using Microsoft.AspNetCore.Mvc;

using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/models")]
    public class VehicleModelApiController : ControllerBase
    {
        private readonly IModelService vehicleModelService;
        private readonly IVehicleModelMapper vehicleModelMapper;
        private readonly IBrandService brandService;

        public VehicleModelApiController(
            IModelService modelService,
            IVehicleModelMapper vehicleModelMapper,
            IBrandService brandService)
        {
            this.vehicleModelService = modelService;
            this.vehicleModelMapper = vehicleModelMapper;
            this.brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicleModel([FromBody] VehicleModelRequestDto dto, CancellationToken cancellationToken)
        {
            var model = this.vehicleModelMapper.Materialize(dto);
            var createdModel = await this.vehicleModelService.CreateAsync(model);

            return Ok(createdModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var models = await this.vehicleModelService.GetAllAsync();
            return Ok(models);
        }

        [HttpGet]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            try
            {
                var model = await this.vehicleModelService.GetByNameAsync(name);
                return Ok(model);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("brandId:guid")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            try
            {
                var model = await this.vehicleModelService.GetByIdAsync(id);
                return Ok(model);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        //api/models/brand?brandName=bmw
        [HttpGet("brand")]
        public async Task<IActionResult> GetAllBrandModels([FromQuery] string brandName)
        {
            try
            {
                var brand = await this.brandService.GetByNameAsync(brandName);
                return Ok(brand.Models);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
