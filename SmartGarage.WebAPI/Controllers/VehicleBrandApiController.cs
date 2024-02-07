using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Services.Contracts;

namespace SmartGarage.WebAPI.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class VehicleBrandApiController : ControllerBase
    {
        private readonly IBrandService brandService;
        private readonly IModelService modelService;

        public VehicleBrandApiController(
            IBrandService brandService,
            IModelService modelService)
        {
            this.brandService = brandService;
            this.modelService = modelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBrandsAsync()
        {
            var brands = await this.brandService.GetAllAsync();

            return Ok(brands);
        }

        //api/brands/models?brandName=bmw
        [HttpGet("models")]
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
