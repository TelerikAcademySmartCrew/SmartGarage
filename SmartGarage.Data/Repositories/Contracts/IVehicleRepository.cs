using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Models;
using Vehicle = SmartGarage.Data.Models.Vehicle;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVehicleRepository
    {
        Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters);
        Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, AppUser currentUser, CancellationToken cancellationToken);
        Task<Vehicle> UpdateVehicleAsync(int vehicleId, Vehicle updatedVehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
