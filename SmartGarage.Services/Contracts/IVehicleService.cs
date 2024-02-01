﻿using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IVehicleService
    {
        Task<IList<VehicleResponseDTO>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters);
        Task<IList<VehicleResponseDTO>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters);
        Task<VehicleResponseDTO> GetVehicleByIdAsync(int vehicleId);
        Task<VehicleResponseDTO> CreateVehicleAsync(VehicleCreateDTO vehicle, AppUser currentUser);
        Task<VehicleResponseDTO> UpdateVehicleAsync(int vehicleId, VehicleCreateDTO updatedVehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
