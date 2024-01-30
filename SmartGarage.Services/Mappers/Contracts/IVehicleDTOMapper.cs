using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services.Mappers.Contracts
{
    public interface IVehicleDTOMapper
    {
        Vehicle Map(VehicleCreateDTO vehicleCreateDTO);
        VehicleResponseDTO Map(Vehicle vehicle);
        IList<VehicleResponseDTO> Map(IList<Vehicle> vehicles);
    }
}
