using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services.Mappers.Contracts
{
    public interface IVehicleDTOMapper
    {
        Vehicle Map(VehicleDTO vehicleCreateDTO);
    }
}
