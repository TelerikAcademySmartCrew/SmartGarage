using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
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

		public async Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters)
		{
            var repairActivitiesToReturn = this.context.RepairActivities
                .Include(ra => ra.RepairActivityType)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
				//var repairActivityType = await this.context.RepairActivityTypes
				//    .FirstOrDefaultAsync(x => x.Name == queryParameters.Name)
				//    ?? throw new EntityNotFoundException(TypeNotFound);

				//repairActivitiesToReturn.Where(x => x.RepairActivityTypeId == repairActivityType.Id);

				repairActivitiesToReturn = repairActivitiesToReturn
                    .Where(x => x.RepairActivityType.Name.Equals(queryParameters.Name));
            }

            if (!string.IsNullOrEmpty(queryParameters.Price))
            {
                repairActivitiesToReturn = repairActivitiesToReturn
                    .Where(x => x.Price.Equals(Convert.ToDouble(queryParameters.Price)));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortByName))
            {
                if (queryParameters.SortByName == "asc")
                {
                    repairActivitiesToReturn = repairActivitiesToReturn
                        .OrderBy(x => x.RepairActivityType.Name);
                }
                else if (queryParameters.SortByName == "desc")
                {
                    repairActivitiesToReturn = repairActivitiesToReturn
                        .OrderByDescending(x => x.RepairActivityType.Name);
                }
            }

            if (!string.IsNullOrEmpty(queryParameters.SortByPrice))
            {
                if (queryParameters.SortByPrice == "asc")
                {
                    repairActivitiesToReturn = repairActivitiesToReturn
                        .OrderBy(x => x.Price);
                }
                else if (queryParameters.SortByPrice == "desc")
                {
                    repairActivitiesToReturn = repairActivitiesToReturn
                        .OrderByDescending(x => x.Price);
                }
            }

            return await repairActivitiesToReturn.ToListAsync();
		}

		public async Task<ICollection<RepairActivity>> GetByVisitId(Guid id)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.VisitId == id)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> GetByNameAsync(string name)
        {
            var repairActivityType = await this.context.RepairActivityTypes
                .FirstOrDefaultAsync(s => s.Name == name)
                ?? throw new EntityNotFoundException(TypeNotFound);

            return await this.context.RepairActivities
                .Where(s => s.RepairActivityTypeId == repairActivityType.Id)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync();
        }

        public async Task<ICollection<RepairActivity>> GetByPriceRangeAsync(int startingPrice, int endingPrice)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.Price >= startingPrice && ra.Price <= endingPrice)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync();
        }

        public async Task<RepairActivity> AddAsync(RepairActivity repairActivity)
        {
            await this.context.RepairActivities.AddAsync(repairActivity);            
            // maybe add it to the list in the visit
            await this.context.SaveChangesAsync();
            return repairActivity;
        }

        public async Task DeleteAsync(RepairActivity repairActivity)
        {
            repairActivity.IsDeleted = true;
            await this.context.SaveChangesAsync();
        }
    }
}
