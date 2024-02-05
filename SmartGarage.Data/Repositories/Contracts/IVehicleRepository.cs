using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using Vehicle = SmartGarage.Data.Models.Vehicle;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVehicleRepository
    {
        Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken);
        Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken);
        Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, AppUser currentUser, CancellationToken cancellationToken);
        Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle, CancellationToken cancellationToken);
        Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken);
    }
}
