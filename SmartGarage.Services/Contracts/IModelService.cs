using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IModelService
    {
        Task<ICollection<VehicleModel>> GetAllAsync();

        Task<VehicleModel> GetByIdAsync(Guid id);

        Task<VehicleModel> GetByNameAsync(string name);

        Task<VehicleModel> CreateAsync(VehicleModel model);
    }
}
