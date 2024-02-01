using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Mappers.Contracts;

namespace SmartGarage.Services.Mappers
{
    public class VehicleDTOMapper : IVehicleDTOMapper
    {
        public Vehicle Map(VehicleCreateDTO vehicleCreateDTO)
        {
            var vehicle = new Vehicle()
            {
                BrandId = vehicleCreateDTO.BrandId,
                ModelId = vehicleCreateDTO.ModelId,
                VIN = vehicleCreateDTO.VIN,
                ProductionYear = vehicleCreateDTO.CreationYear,
                LicensePlateNumber = vehicleCreateDTO.LicensePlate,
            };
            return vehicle;
        }

        public VehicleResponseDTO Map(Vehicle vehicle)
        {
            var vehicleResponseDto = new VehicleResponseDTO()
            {
                Brand = vehicle.Brand.Name,
                Model = vehicle.Model.Name,
                VIN = vehicle.VIN,
                CreationYear = vehicle.ProductionYear,
                LicensePlate = vehicle.LicensePlateNumber,
            };
            return vehicleResponseDto;
        }

        public IList<VehicleResponseDTO> Map(IList<Vehicle> vehicles)
        {
            return vehicles.Select(vehicle => Map(vehicle)).ToList();
        }
    }
}
