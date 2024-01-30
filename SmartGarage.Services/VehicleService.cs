using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using SmartGarage.Services.Mappers;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Vehicle> CreateVehicleAsync(VehicleDTO vehicleDTO, AppUser currentUser)
        {
            var vehicle = this.vehicleDTOMapper.Map(vehicleDTO);
            return await vehicleRepository.CreateVehicleAsync(vehicle, currentUser);
        }

        public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters)
        {
            return await vehicleRepository.GetAllAsync(vehicleQueryParameters);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await vehicleRepository.GetVehicleByIdAsync(vehicleId);
        }

        public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters)
        {
            return await vehicleRepository.GetVehiclesByUserAsync(userId, vehicleQueryParameters);
        }

        public async Task<Vehicle> UpdateVehicleAsync(int vehicleId, VehicleDTO updatedVehicleDTO)
        {
            var updatedVehicle = this.vehicleDTOMapper.Map(updatedVehicleDTO);
            return await vehicleRepository.UpdateVehicleAsync(vehicleId, updatedVehicle);
        }
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            await vehicleRepository.DeleteVehicleAsync(vehicleId);
        }
    }
}
