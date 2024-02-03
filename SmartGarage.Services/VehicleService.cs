using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;
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

    public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters)
    {
        return await vehicleRepository.GetAllAsync(vehicleQueryParameters);
    }

    public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId)
    {
        return await vehicleRepository.GetVehicleByIdAsync(vehicleId);
    }

    public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters)
    {
        return await vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
    }

    public async Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle)
    {
        return await vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle);
    }
    public async Task DeleteVehicleAsync(Guid vehicleId)
    {
        await vehicleRepository.DeleteVehicleAsync(vehicleId);
    }

}