﻿using Microsoft.EntityFrameworkCore;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.ApplicationErrorMessages.Service;

namespace SmartGarage.Data.Repositories
{
	public class CarServiceRepository : ICarServiceRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CarServiceRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            this.applicationDbContext.Services.Add(service);           

            await this.applicationDbContext.SaveChangesAsync();
            return service;
        }

        public async Task<IList<Service>> GetAllAsync()
        {
            return await this.applicationDbContext.Services.ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await this.applicationDbContext.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId)
                ?? throw new ArgumentNullException(ServiceNotFound);
        }

        //public async Task<IList<Service>> GetServicesByUserIdAsync(string userId)
        //{
        //    return await this.applicationDbContext.Services
        //        .Where(s => s.UserId == userId)
        //        .ToListAsync();
        //}

        //public async Task<IList<Service>> GetServicesByVehicleIdAsync(int vehicleId)
        //{
        //    return await this.applicationDbContext.Services
        //        .Where(s => s.VehicleId == vehicleId)
        //        .ToListAsync();
        //}

        public async Task<Service> UpdateServiceAsync(int serviceId, Service service)
        {
            var serviceToUpdate = await this.GetServiceByIdAsync(serviceId);

            // todo:

            await this.applicationDbContext.SaveChangesAsync();
            return serviceToUpdate;
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var serviceToDelete = await this.GetServiceByIdAsync(serviceId);

            this.applicationDbContext.Services.Remove(serviceToDelete);
            await this.applicationDbContext.SaveChangesAsync();           
        } 
    }
}