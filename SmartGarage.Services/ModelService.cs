using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.VehicleBrand;

namespace SmartGarage.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            this.modelRepository = modelRepository;
        }

        public async Task<ICollection<VehicleModel>> GetAllAsync()
        {
            return await this.modelRepository.GetAllAsync();
        }

        public async Task<VehicleModel> GetByIdAsync(Guid id)
        {
            return await this.modelRepository.GetByIdAsync(id);
        }

        public async Task<VehicleModel> GetByNameAsync(string name)
        {
            return await this.modelRepository.GetByNameAsync(name);
        }

        public async Task<VehicleModel> CreateAsync(VehicleModel model)
        {
            return await this.modelRepository.CreateAsync(model);
        }
    }
}
