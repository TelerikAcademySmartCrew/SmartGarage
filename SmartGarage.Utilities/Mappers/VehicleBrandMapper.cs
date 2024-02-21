using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers
{
    public class VehicleBrandMapper : IVehicleBrandMapper
    {
        public VehicleBrand Materialize(VehicleBrandRequestDto dto)
        {
            return new VehicleBrand
            {
                Name = dto.Name
            };
        }
    }
}
