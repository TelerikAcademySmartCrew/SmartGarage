using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Utilities.Mappers;

public class VehicleMapper : IVehicleMapper
{
    public Vehicle MaterializeInputModel(VehicleInputModel vehicleCreateDto)
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

    public VehicleViewModel ToViewModel(Vehicle vehicle)
    {
        var vehicleViewModel = new VehicleViewModel()
        {
            Brand = vehicle.Brand.Name,
            Model = vehicle.Model.Name,
            VIN = vehicle.VIN,
            CreationYear = vehicle.ProductionYear,
            LicensePlate = vehicle.LicensePlateNumber,
            Username = vehicle.User.UserName,
        };
        return vehicleViewModel;
    }

    public IList<VehicleViewModel> ToViewModel(IEnumerable<Vehicle> vehicles)
    {
        return vehicles.Select(ToViewModel).ToList();
    }
}