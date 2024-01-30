using Microsoft.EntityFrameworkCore;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private const string VehicleNotFoundMessage = "Vehicle not found!";
        private readonly ApplicationDbContext applicationDbContext;

        public VehicleRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, AppUser currentUser)
        {
            vehicle.UserId = "d1499578-6274-48aa-b4f7-495baba0721d";
            applicationDbContext.Vehicles.Add(vehicle);
            var brand = await applicationDbContext.VehicleBrands.FirstOrDefaultAsync(b => b.Id == 1);
            var model = await applicationDbContext.VehicleModels.FirstOrDefaultAsync(b => b.Id == 1);
            model.Vehicles.Add(vehicle);
            brand.Vehicles.Add(vehicle);
            currentUser.Vehicles.Add(vehicle);
            await applicationDbContext.SaveChangesAsync();
            return vehicle;
        }

        public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = applicationDbContext.Vehicles.AsQueryable();

            vehiclesToReturn = FilterVehiclesByQuery(vehicleQueryParameters, vehiclesToReturn);

            return await vehiclesToReturn.ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
        {
            return await applicationDbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Id == vehicleId)
                ?? throw new ArgumentNullException(VehicleNotFoundMessage);
        }

        public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = applicationDbContext.Vehicles
                .Where(v => v.UserId == userId)
                .AsQueryable();

            vehiclesToReturn = FilterVehiclesByQuery(vehicleQueryParameters, vehiclesToReturn);

            return await vehiclesToReturn.ToListAsync();
        }

        public async Task<Vehicle> UpdateVehicleAsync(int vehicleId, Vehicle updatedVehicle)
        {
            var vehicleToUpdate = await GetVehicleByIdAsync(vehicleId);

            vehicleToUpdate.BrandId = updatedVehicle.BrandId;
            vehicleToUpdate.ModelId = updatedVehicle.ModelId;
            vehicleToUpdate.UserId = updatedVehicle.UserId;

            await applicationDbContext.SaveChangesAsync();
            return vehicleToUpdate;
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await GetVehicleByIdAsync(vehicleId);
            vehicle.IsDeleted = true;
            await applicationDbContext.SaveChangesAsync();
        }

        private static IQueryable<Vehicle> FilterVehiclesByQuery(VehicleQueryParameters vehicleQueryParameters, IQueryable<Vehicle> vehiclesToReturn)
        {
            if (!string.IsNullOrEmpty(vehicleQueryParameters.Brand))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.Brand.Name == vehicleQueryParameters.Brand);
            }

            if (!string.IsNullOrEmpty(vehicleQueryParameters.Model))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.Model.Name == vehicleQueryParameters.Model);
            }

            if (!string.IsNullOrEmpty(vehicleQueryParameters.Username))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.User.UserName == vehicleQueryParameters.Username);
            }

            if (!string.IsNullOrEmpty(vehicleQueryParameters.VIN))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.VIN == vehicleQueryParameters.VIN);
            }

            if (!string.IsNullOrEmpty(vehicleQueryParameters.LicensePlate))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.LicensePlate == vehicleQueryParameters.LicensePlate);
            }

            return vehiclesToReturn;
        }
    }
}
