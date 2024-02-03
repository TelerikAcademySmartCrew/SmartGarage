using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Mappers.Contracts;

namespace SmartGarage.Services.Mappers;

public class VehicleDtoMapper : IVehicleDtoMapper
{
    public Vehicle Map(VehicleCreateDTO vehicleCreateDto)
    {
        var vehicle = new Vehicle()
        {
            BrandId = vehicleCreateDto.BrandId,
            ModelId = vehicleCreateDto.ModelId,
            VIN = vehicleCreateDto.VIN,
            ProductionYear = vehicleCreateDto.CreationYear,
            LicensePlateNumber = vehicleCreateDto.LicensePlate,
        };
        return vehicle;
    }

    public VehicleResponseDto Map(Vehicle vehicle)
    {
        var vehicleResponseDto = new VehicleResponseDto()
        {
            Brand = vehicle.Brand.Name,
            Model = vehicle.Model.Name,
            VIN = vehicle.VIN,
            CreationYear = vehicle.ProductionYear,
            LicensePlate = vehicle.LicensePlateNumber,
            Username = vehicle.User.UserName,
        };
        return vehicleResponseDto;
    }

    public IList<VehicleResponseDto> Map(IEnumerable<Vehicle> vehicles)
    {
        return vehicles.Select(Map).ToList();
    }
}