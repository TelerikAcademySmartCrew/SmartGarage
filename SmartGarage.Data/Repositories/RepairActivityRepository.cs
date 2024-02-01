using Microsoft.EntityFrameworkCore;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
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
            var repairActivitiesToReturn = this.context.RepairActivities.AsQueryable();

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
