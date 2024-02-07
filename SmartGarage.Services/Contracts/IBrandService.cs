using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IBrandService
    {
        Task<ICollection<VehicleBrand>> GetAllAsync();

        Task<VehicleBrand> GetByIdAsync(Guid id);

        Task<VehicleBrand> GetByNameAsync(string name);

        Task<VehicleBrand> CreateAsync(VehicleBrand brand);
    }
}
