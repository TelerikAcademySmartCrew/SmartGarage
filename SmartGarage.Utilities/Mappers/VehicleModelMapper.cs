using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers
{
    public class VehicleModelMapper : IVehicleModelMapper
    {
        public VehicleModel Materialize(VehicleModelRequestDto model)
        {
            return new VehicleModel
            {
                Name = model.Name,
                BrandId = model.BrandId
            };
        }
    }
}
