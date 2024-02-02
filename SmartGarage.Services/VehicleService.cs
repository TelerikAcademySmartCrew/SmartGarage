using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.Data.Models;

namespace SmartGarage.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleDTOMapper vehicleDtoMapper;
        private readonly UserManager<AppUser> userManager;

        public VehicleService(IVehicleRepository vehicleRepository,
            IVehicleDTOMapper vehicleDtoMapper,
            UserManager<AppUser> userManager)
        {
            this.vehicleRepository = vehicleRepository;
            this.vehicleDtoMapper = vehicleDtoMapper;
            this.userManager = userManager;
        }

        public async Task<VehicleResponseDTO> CreateVehicleAsync(VehicleCreateDTO vehicleDto, string email, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            var vehicle = this.vehicleDtoMapper.Map(vehicleDto);
            vehicle.UserId = user.Id;
            return this.vehicleDtoMapper.Map(await vehicleRepository.CreateVehicleAsync(vehicle, user, cancellationToken));
        }

        public async Task<IList<VehicleResponseDTO>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters)
        {
            var vehicles = await vehicleRepository.GetAllAsync(vehicleQueryParameters);
            return this.vehicleDtoMapper.Map(vehicles);
        }

        public async Task<VehicleResponseDTO> GetVehicleByIdAsync(int vehicleId)
        {
            return this.vehicleDtoMapper.Map(await vehicleRepository.GetVehicleByIdAsync(vehicleId));
        }

        public async Task<IList<VehicleResponseDTO>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = await vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
            return this.vehicleDtoMapper.Map(vehiclesToReturn);
        }

        public async Task<VehicleResponseDTO> UpdateVehicleAsync(int vehicleId, VehicleCreateDTO updatedVehicleDto)
        {
            var updatedVehicle = this.vehicleDtoMapper.Map(updatedVehicleDto);
            return this.vehicleDtoMapper.Map(await vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle));
        }
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            await vehicleRepository.DeleteVehicleAsync(vehicleId);
        }

    }
}
