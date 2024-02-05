using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ResponseDtos;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVehicleMapper
    {
        Vehicle MaterializeInputModel(VehicleInputModel vehicleInputModel);
        VehicleViewModel ToViewModel(Vehicle vehicle);
        IList<VehicleViewModel> ToViewModel(IEnumerable<Vehicle> vehicles);
        Vehicle MaterializeRequestDto(VehicleRequestDto vehicleRequestDto);
        VehicleResponseDto ToResponseDto(Vehicle vehicle);
        IList<VehicleResponseDto> ToResponseDto(IEnumerable<Vehicle> vehicles);
    }
}
