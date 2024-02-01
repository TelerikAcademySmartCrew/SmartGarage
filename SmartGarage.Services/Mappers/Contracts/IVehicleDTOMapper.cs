using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;

namespace SmartGarage.Services.Mappers.Contracts
{
    public interface IVehicleDTOMapper
    {
        Vehicle Map(VehicleCreateDTO vehicleCreateDTO);
        VehicleResponseDTO Map(Vehicle vehicle);
        IList<VehicleResponseDTO> Map(IList<Vehicle> vehicles);
    }
}
