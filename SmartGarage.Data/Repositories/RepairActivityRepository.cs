using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.RepairActivity;

namespace SmartGarage.Data.Repositories
{
    public class RepairActivityRepository : IRepairActivityRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public RepairActivityRepository(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }        

        public async Task<ICollection<RepairActivity>> GetByVisitId(int id)
        {
            return await this.applicationDbContext.RepairActivities
                .Where(ra => ra.VisitId == id)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> GetByNameAsync(string name)
        {
            var serviceType = await this.applicationDbContext.RepairActivityTypes
                .FirstOrDefaultAsync(s => s.Name == name)
                ?? throw new EntityNotFoundException(TypeNotFound);

            return await this.applicationDbContext.RepairActivities
                .Where(s => s.RepairActivityTypeId == serviceType.Id)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivities)
        {
            this.applicationDbContext.RepairActivities.AddRange(repairActivities);

            await this.applicationDbContext.SaveChangesAsync();
            return repairActivities;
        }
    }
}
