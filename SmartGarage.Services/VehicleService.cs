using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleDTOMapper vehicleDTOMapper;
        private readonly UserManager<AppUser> userManager;

        public VehicleService(IVehicleRepository vehicleRepository,
            IVehicleDTOMapper vehicleDTOMapper,
            UserManager<AppUser> userManager)
        {
            this.vehicleRepository = vehicleRepository;
            this.vehicleDTOMapper = vehicleDTOMapper;
            this.userManager = userManager;
        }

        public async Task<VehicleResponseDTO> CreateVehicleAsync(VehicleCreateDTO vehicleDTO, string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            var vehicle = this.vehicleDTOMapper.Map(vehicleDTO);
            vehicle.UserId = user.Id;
            return this.vehicleDTOMapper.Map(await vehicleRepository.CreateVehicleAsync(vehicle, user));
        }

        public async Task<IList<VehicleResponseDTO>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters)
        {
            var vehicles = await vehicleRepository.GetAllAsync(vehicleQueryParameters);
            return this.vehicleDTOMapper.Map(vehicles);
        }

        public async Task<VehicleResponseDTO> GetVehicleByIdAsync(int vehicleId)
        {
            return this.vehicleDTOMapper.Map(await vehicleRepository.GetVehicleByIdAsync(vehicleId));
        }

        public async Task<IList<VehicleResponseDTO>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = await vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
            return this.vehicleDTOMapper.Map(vehiclesToReturn);
        }

        public async Task<VehicleResponseDTO> UpdateVehicleAsync(int vehicleId, VehicleCreateDTO updatedVehicleDTO)
        {
            var updatedVehicle = this.vehicleDTOMapper.Map(updatedVehicleDTO);
            return this.vehicleDTOMapper.Map(await vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle));
        }
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            await vehicleRepository.DeleteVehicleAsync(vehicleId);
        }

    }
}
