using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IBrandRepository
    {
        Task<ICollection<VehicleBrand>> GetAllAsync();

        Task<VehicleBrand> GetByIdAsync(Guid id );

        Task<VehicleBrand> GetByNameAsync(string name);

        Task<VehicleBrand> CreateAsync(VehicleBrand brand);
    }
}
