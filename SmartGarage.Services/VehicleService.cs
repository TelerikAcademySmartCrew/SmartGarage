using Microsoft.AspNetCore.Identity;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Common.Exceptions;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
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

        public async Task<Vehicle> GetVehicleByVinAsync(string vin, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.GetVehicleByVinAsync(vin, cancellationToken);
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, string email, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByEmailAsync(email)
                ?? throw new EntityNotFoundException($"User not found.");

            vehicle.UserId = user.Id;

            return await this.vehicleRepository.CreateVehicleAsync(vehicle, user, cancellationToken);
        }

        public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.GetAllAsync(vehicleQueryParameters, cancellationToken);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.GetVehicleByIdAsync(vehicleId, cancellationToken);
        }

        public async Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlate, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.GetVehicleByLicensePlateAsync(licensePlate, cancellationToken);
        }

        public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters, cancellationToken);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle, CancellationToken cancellationToken)
        {
            return await this.vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle, cancellationToken);
        }
        public async Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            await this.vehicleRepository.DeleteVehicleAsync(vehicleId, cancellationToken);
        }

    }
}