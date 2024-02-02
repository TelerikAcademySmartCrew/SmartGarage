using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IVehicleService
    {
        Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters);
        Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, string email, CancellationToken cancellationToken);
        Task<Vehicle> UpdateVehicleAsync(int vehicleId, Vehicle updatedVehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
