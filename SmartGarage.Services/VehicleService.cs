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
        private readonly IVehicleDTOMapper vehicleDTOMapper;

        public VehicleService(IVehicleRepository vehicleRepository,
            IVehicleDTOMapper vehicleDTOMapper)
        {
            this.vehicleRepository = vehicleRepository;
            this.vehicleDTOMapper = vehicleDTOMapper;
        }

        public async Task<VehicleResponseDTO> CreateVehicleAsync(VehicleCreateDTO vehicleDTO, AppUser currentUser)
        {
            var vehicle = this.vehicleDTOMapper.Map(vehicleDTO);
            return this.vehicleDTOMapper.Map(await vehicleRepository.CreateVehicleAsync(vehicle, currentUser));
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
