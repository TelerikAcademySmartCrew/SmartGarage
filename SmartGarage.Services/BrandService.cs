using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            this.brandRepository = brandRepository;
        }        

        public async Task<ICollection<VehicleBrand>> GetAllAsync()
        {
            return await this.brandRepository.GetAllAsync();
        }

        public async Task<VehicleBrand> GetByIdAsync(Guid id)
        {
            return await this.brandRepository.GetByIdAsync(id);
        }

        public async Task<VehicleBrand> GetByNameAsync(string name)
        {
            return await this.brandRepository.GetByNameAsync(name);
        }

        public async Task<VehicleBrand> CreateAsync(VehicleBrand brand)
        {
            return await this.brandRepository.CreateAsync(brand);
        }
    }
}
