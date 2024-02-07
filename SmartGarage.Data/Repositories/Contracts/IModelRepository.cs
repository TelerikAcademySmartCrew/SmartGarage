using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IModelRepository
    {
        Task<ICollection<VehicleModel>> GetAllAsync();

        Task<VehicleModel> GetByIdAsync(Guid id);

        Task<VehicleModel> GetByNameAsync(string name);

        Task<VehicleModel> CreateAsync(VehicleModel model);
    }
}
