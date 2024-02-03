using Microsoft.EntityFrameworkCore;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Data.Models;

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


        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle, AppUser currentUser, CancellationToken cancellationToken)
        {
            applicationDbContext.Vehicles.Add(vehicle);
            currentUser.Vehicles.Add(vehicle);
            await AssignBrandModelToVehicleAsync(vehicle, cancellationToken);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return vehicle;
        }

        public async Task<IList<Vehicle>> GetAllAsync(VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
        {
            var vehiclesToReturn = applicationDbContext.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.User)
                .Where(v => !v.IsDeleted)
                .AsQueryable();

            vehiclesToReturn = FilterVehiclesByQuery(vehicleQueryParameters, vehiclesToReturn);

            return await vehiclesToReturn.ToListAsync(cancellationToken);
        }

        public async Task<Vehicle> GetVehicleByIdAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await applicationDbContext.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.User)
                .Where(v => !v.IsDeleted)
                .FirstOrDefaultAsync(v => v.Id == vehicleId, cancellationToken)
                ?? throw new EntityNotFoundException(VehicleNotFoundMessage);
        }

        public async Task<IList<Vehicle>> GetVehiclesByUserAsync(string userId, VehicleQueryParameters vehicleQueryParameters, CancellationToken cancellationToken)
        {
            var vehiclesToReturn = applicationDbContext.Vehicles
                .Include(v => v.Brand)
                .Include(v => v.Model)
                .Include(v => v.User)
                .Where(v => !v.IsDeleted)
                .Where(v => v.UserId == userId)
                .AsQueryable();

            FilterVehiclesByQuery(vehicleQueryParameters, vehiclesToReturn);

            return await vehiclesToReturn.ToListAsync(cancellationToken);
        }

        public async Task<Vehicle> UpdateVehicleAsync(Guid vehicleId, Vehicle updatedVehicle, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await GetVehicleByIdAsync(vehicleId, cancellationToken);

            vehicleToUpdate.BrandId = updatedVehicle.BrandId;
            vehicleToUpdate.ModelId = updatedVehicle.ModelId;
            vehicleToUpdate.LicensePlateNumber = updatedVehicle.LicensePlateNumber;
            vehicleToUpdate.VIN = updatedVehicle.VIN;
            vehicleToUpdate.ProductionYear = updatedVehicle.ProductionYear;

            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return vehicleToUpdate;
        }

        public async Task DeleteVehicleAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            var vehicle = await GetVehicleByIdAsync(vehicleId, cancellationToken);
            vehicle.IsDeleted = true;
            await applicationDbContext.SaveChangesAsync(cancellationToken);
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

            if (!string.IsNullOrEmpty(vehicleQueryParameters.VIN))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.VIN == vehicleQueryParameters.VIN);
            }
            
            if (!string.IsNullOrEmpty(vehicleQueryParameters.LicensePlate))
            {
                vehiclesToReturn = vehiclesToReturn.Where(v => v.LicensePlateNumber == vehicleQueryParameters.LicensePlate);
            }

            return vehiclesToReturn;
        }
        
        private async Task AssignBrandModelToVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            var brand = await this.applicationDbContext.VehicleBrands.FirstOrDefaultAsync(b => b.Id == vehicle.BrandId, cancellationToken);
            var model = await this.applicationDbContext.VehicleModels.FirstOrDefaultAsync(m => m.Id == vehicle.ModelId, cancellationToken);
            brand?.Vehicles.Add(vehicle);
            model?.Vehicles.Add(vehicle);
        }

    }
}
