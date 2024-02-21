using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVehicleModelMapper
    {
        VehicleModel Materialize(VehicleModelRequestDto model);        
    }
}
