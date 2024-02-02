using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;

namespace SmartGarage.Services.Mappers.Contracts
{
    public interface IVehicleDtoMapper
    {
        Vehicle Map(VehicleCreateDTO vehicleCreateDto);
        VehicleResponseDto Map(Vehicle vehicle);
        IList<VehicleResponseDto> Map(IEnumerable<Vehicle> vehicles);
    }
}
