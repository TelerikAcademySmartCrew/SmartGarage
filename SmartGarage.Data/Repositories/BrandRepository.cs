using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.VehicleBrand;

namespace SmartGarage.Data.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext context;

        public BrandRepository(ApplicationDbContext context)
        {
            this.context = context;
        }        

        public async Task<ICollection<VehicleBrand>> GetAllAsync()
        {
            return await this.context.VehicleBrands
                .Include(x => x.Models)
                .ToListAsync();
        }

        public async Task<VehicleBrand> GetByIdAsync(Guid id)
        {
            return await this.context.VehicleBrands
                .Include(x => x.Models)
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(BrandNotFound);
        }

        public async Task<VehicleBrand> GetByNameAsync(string name)
        {
            return await this.context.VehicleBrands
                .Include(x => x.Models)
                .FirstOrDefaultAsync(x => x.Name == name)                
                ?? throw new EntityNotFoundException(BrandNotFound);
        }

        public async Task<VehicleBrand> CreateAsync(VehicleBrand brand)
        {
            if (await this.context.VehicleBrands.AnyAsync(x => x.Name == brand.Name))
            {
                throw new EntityAlreadyExistsException(BrandAlreadyExists);
            }

            await this.context.AddAsync(brand);
            await this.context.SaveChangesAsync();

            return brand;
        }
    }
}
