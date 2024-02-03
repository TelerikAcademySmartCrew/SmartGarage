using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Data.Models;

namespace SmartGarage.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository vehicleRepository;
    private readonly UserManager<AppUser> userManager;

    public VehicleService(IVehicleRepository vehicleRepository,
        UserManager<AppUser> userManager)
    {
        this.vehicleRepository = vehicleRepository;
        this.userManager = userManager;
    }

    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, string email, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByEmailAsync(email);
        vehicle.UserId = user.Id;
        return await vehicleRepository.CreateVehicleAsync(vehicle, user, cancellationToken);
    }

    public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
    {
        return await vehicleRepository.GetAllAsync(vehicleQueryParameters, cancellationToken);
    }

    public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        return await vehicleRepository.GetVehicleByIdAsync(vehicleId, cancellationToken);
    }

    public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
    {
        return await vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken);
    }

    public async Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle, CancellationToken cancellationToken)
    {
        return await vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle, cancellationToken);
    }
    public async Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken)
    {
        await vehicleRepository.DeleteVehicleAsync(vehicleId, cancellationToken);
    }

}