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


        public Vehicle CreateVehicle(Vehicle vehicle, AppUser currentUser)
        {
            this.applicationDbContext.Vehicles.Add(vehicle);
            currentUser.Vehicles.Add(vehicle);
            applicationDbContext.SaveChanges();
            return vehicle;
        }
        public IList<Vehicle> GetAll(VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = this.applicationDbContext.Vehicles
                .AsQueryable();

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

            return vehiclesToReturn.ToList();
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            return applicationDbContext.Vehicles
                .FirstOrDefault(v => v.Id == vehicleId)
                ?? throw new ArgumentNullException(VehicleNotFoundMessage);
            //TODO: Implement custom exceptions
        }

        public IList<Vehicle> GetVehiclesByUser(string userId, VehicleQueryParameters vehicleQueryParameters)
        {
            var vehiclesToReturn = this.applicationDbContext.Vehicles
                .Where(v => v.UserId == userId)
                .AsQueryable();

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

            return vehiclesToReturn.ToList();
        }

        public Vehicle UpdateVehicle(int vehicleId, Vehicle updatedVehicle)
        {
            var vehicleToUpdate = this.GetVehicleById(vehicleId);

            vehicleToUpdate.BrandId = updatedVehicle.BrandId;
            vehicleToUpdate.ModelId = updatedVehicle.ModelId;
            vehicleToUpdate.UserId = updatedVehicle.UserId;

            applicationDbContext.SaveChanges();
            return vehicleToUpdate;
        }
        public void DeleteVehicle(int vehicleId)
        {
            var vehicle = this.GetVehicleById(vehicleId);
            vehicle.IsDeleted = true;
            applicationDbContext.SaveChanges();
        }
    }
}
