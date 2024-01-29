using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IServiceRepository
    {
        Task<IList<Service>> GetAllAsync(ServiceQueryParameters serviceQueryParameters);
        Task<Service> GetServiceByIdAsync(int serviceId);
        Task<IList<Service>> GetServicesByUserIdAsync(string userId);
        Task<IList<Service>> GetServicesByVehicleIdAsync(int vehicleId);
        Task<Service> CreateServiceAsync(Service service, AppUser currentUser);
        Task<Service> UpdateServiceAsync(int serviceId, Service service);
        Task DeleteServiceAsync(int serviceId);
    }
}
