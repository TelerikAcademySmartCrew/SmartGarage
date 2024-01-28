using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVehicleRepository
    {
        Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters);
        Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, AppUser currentUser);
        Task<Vehicle> UpdateVehicleAsync(int vehicleId, Vehicle updatedVehicle);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
