using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface ICarServiceRepository
    {
        Task<IList<Service>> GetAllAsync();

        Task<Service> GetServiceByIdAsync(int serviceId);

        //Task<IList<Service>> GetServicesByUserIdAsync(string userId);

        //Task<IList<Service>> GetServicesByVehicleIdAsync(int vehicleId);

        Task<Service> CreateServiceAsync(Service service);

        Task<Service> UpdateServiceAsync(int serviceId, Service service);

        Task DeleteServiceAsync(int serviceId);
    }
}
