using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVehicleMapper
    {
        Vehicle MaterializeInputModel(VehicleInputModel vehicleInputModel);
        VehicleViewModel ToViewModel(Vehicle vehicle);
        IList<VehicleViewModel> ToViewModel(IEnumerable<Vehicle> vehicles);
    }
}
