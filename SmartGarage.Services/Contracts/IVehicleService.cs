using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Services.Contracts
{
    public interface IVehicleService
    {
        Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters);
        Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> CreateVehicleAsync(VehicleDTO vehicle, AppUser currentUser);
        Task<Vehicle> UpdateVehicleAsync(int vehicleId, VehicleDTO updatedVehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
