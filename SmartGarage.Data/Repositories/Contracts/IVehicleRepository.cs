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
        IList<Vehicle> GetAll(VehicleQueryParameters vehicleQueryParameters);
        IList<Vehicle> GetVehiclesByUser(string userId, VehicleQueryParameters vehicleQueryParameters);
        Vehicle GetVehicleById(int vehicleId);
        Vehicle CreateVehicle(Vehicle vehicle, AppUser currentUser);
        Vehicle UpdateVehicle(int vehicleId, Vehicle updatedVehicle);
        void DeleteVehicle(int vehicleId);
    }
}
