using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.VehicleModel;

namespace SmartGarage.Data.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext context;

        public ModelRepository(ApplicationDbContext context)
        {
            this.context = context;
        }        

        public async Task<ICollection<VehicleModel>> GetAllAsync()
        {
            return await this.context.VehicleModels
                .Include(x => x.Brand)
                .ToListAsync();
        }

        public async Task<VehicleModel> GetByIdAsync(Guid id)
        {
            return await this.context.VehicleModels
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(ModelNotFound);
        }

        public async Task<VehicleModel> GetByNameAsync(string name)
        {
            return await this.context.VehicleModels
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.Name == name)
                ?? throw new EntityNotFoundException(ModelNotFound);
        }

        public async Task<VehicleModel> CreateAsync(VehicleModel model)
        {
            await this.context.AddAsync(model);
            await this.context.SaveChangesAsync();

            return model;
        }
    }
}
