using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.RepairActivity;

namespace SmartGarage.Data.Repositories
{
    public class RepairActivityRepository : IRepairActivityRepository
    {
        private readonly ApplicationDbContext context;

        public RepairActivityRepository(ApplicationDbContext applicationDbContext) 
        {
            this.context = applicationDbContext;
        }        

        public async Task<ICollection<RepairActivity>> GetByVisitId(int id)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.VisitId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> GetByNameAsync(string name)
        {
            var serviceType = await this.context.RepairActivityTypes
                .FirstOrDefaultAsync(s => s.Name == name)
                ?? throw new EntityNotFoundException(TypeNotFound);

            return await this.context.RepairActivities
                .Where(s => s.RepairActivityTypeId == serviceType.Id)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.Price >= startingPrice && ra.Price <= endingPrice)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivities)
        {
            this.context.RepairActivities.AddRange(repairActivities);

            await this.context.SaveChangesAsync();
            return repairActivities;
        }
    }
}
