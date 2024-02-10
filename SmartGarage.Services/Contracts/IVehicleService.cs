using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Services.Contracts;

public interface IVehicleService
{
    Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken);
    Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken);
    Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken);
    Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken);
    Task<Vehicle> GetVehicleByVinAsync(string vin, CancellationToken cancellationToken);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, string email, CancellationToken cancellationToken);
    Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle, CancellationToken cancellationToken);
    Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken);
}