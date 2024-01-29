using Microsoft.EntityFrameworkCore;
using SmartGarage.Data.Models;
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
    public class ServiceRepository : IServiceRepository
    {
        private const string ServiceNotFoundError = "Service not found!";
        private readonly ApplicationDbContext applicationDbContext;

        public ServiceRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Service> CreateServiceAsync(Service service, AppUser currentUser)
        {
            applicationDbContext.Services.Add(service);
            currentUser.Services.Add(service);
            await applicationDbContext.SaveChangesAsync();
            return service;
        }

        public async Task<IList<Service>> GetAllAsync(ServiceQueryParameters serviceQueryParameters)
        {
            var servicesToReturn = applicationDbContext.Services.AsQueryable();

            if (!string.IsNullOrEmpty(serviceQueryParameters.Name))
            {
                servicesToReturn = servicesToReturn.Where(s => s.Name == serviceQueryParameters.Name);
            }

            return await servicesToReturn.ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            return await applicationDbContext.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId)
                ?? throw new ArgumentNullException(ServiceNotFoundError);
        }

        public Task<IList<Service>> GetServicesByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Service>> GetServicesByVehicleIdAsync(int serviceId)
        {
            throw new NotImplementedException();
        }

        public Task<Service> UpdateServiceAsync(int serviceId, Service service)
        {
            throw new NotImplementedException();
        }
        public Task DeleteServiceAsync(int serviceId)
        {
            throw new NotImplementedException();
        }

       
    }
}
