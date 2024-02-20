using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ResponseDtos;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers;

public class VehicleMapper : IVehicleMapper
{
    public Vehicle MaterializeInputModel(VehicleInputModel vehicleInputModel)
    {
        var vehicle = new Vehicle()
        {
            BrandId = vehicleInputModel.BrandId,
            ModelId = vehicleInputModel.ModelId,
            VIN = vehicleInputModel.VIN,
            ProductionYear = vehicleInputModel.CreationYear,
            LicensePlateNumber = vehicleInputModel.LicensePlate,
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

    public Vehicle MaterializeRequestDto(VehicleRequestDto vehicleRequestDto)
    {
        var vehicle = new Vehicle()
        {
            BrandId = vehicleRequestDto.BrandId,
            ModelId = vehicleRequestDto.ModelId,
            VIN = vehicleRequestDto.VIN,
            ProductionYear = vehicleRequestDto.CreationYear,
            LicensePlateNumber = vehicleRequestDto.LicensePlate,
        };
        return vehicle;
    }

    public VehicleResponseDto ToResponseDto(Vehicle vehicle)
    {
        var vehicleResponse = new VehicleResponseDto
        {
            Brand = vehicle.Brand.Name,
            Model = vehicle.Model.Name,
            VIN = vehicle.VIN,
            CreationYear = vehicle.ProductionYear,
            LicensePlate = vehicle.LicensePlateNumber,
            Username = vehicle.User.UserName,
        };
        return vehicleResponse;
    }

    public IList<VehicleResponseDto> ToResponseDto(IEnumerable<Vehicle> vehicles)
    {
        return vehicles.Select(ToResponseDto).ToList();
    }

    public RegisterdVehicleInfoViewModel VehicleDataToRegisterdVehicleDataViewModel(RegisterVehicleViewModel vehileRegisterData)
    {
        var registeredVehicleData = new RegisterdVehicleInfoViewModel
        {
            RegisterBrand = vehileRegisterData.RegisterBrand,
            RegisterModel = vehileRegisterData.RegisterModel,
            VIN = vehileRegisterData.VIN,
            CreationYear = vehileRegisterData.CreationYear,
            LicensePlate = vehileRegisterData.LicensePlate,
            CustomerEmail = vehileRegisterData.CustomerEmail
        };
        return registeredVehicleData;
    }
}