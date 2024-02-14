using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using System.Threading;
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

        public async Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters,
            CancellationToken cancellationToken)
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

            return await repairActivitiesToReturn.ToListAsync(cancellationToken);
        }

        public async Task<ICollection<RepairActivity>> GetByVisitId(Guid id, CancellationToken cancellationToken)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.VisitId == id && !ra.IsDeleted)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<RepairActivity>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var repairActivityType = await this.context.RepairActivityTypes
                .FirstOrDefaultAsync(s => s.Name == name)
                ?? throw new EntityNotFoundException(TypeNotFound);

            return await this.context.RepairActivities
                .Where(ra => ra.RepairActivityTypeId == repairActivityType.Id && !ra.IsDeleted)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync(cancellationToken);
        }

        public async Task<ICollection<RepairActivity>> GetByPriceRangeAsync(int startingPrice, int endingPrice, CancellationToken cancellationToken)
        {
            return await this.context.RepairActivities
                .Where(ra => ra.Price >= startingPrice && ra.Price <= endingPrice && !ra.IsDeleted)
                .Include(ra => ra.RepairActivityType)
                .ToListAsync(cancellationToken);
        }

        public async Task<RepairActivity> AddAsync(RepairActivity repairActivity, CancellationToken cancellationToken)
        {
            await this.context.RepairActivities.AddAsync(repairActivity, cancellationToken);
            // maybe add it to the list in the visit
            await this.context.SaveChangesAsync(cancellationToken);
            return repairActivity;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var repairActivity = await this.GetById(id);
            repairActivity.IsDeleted = true;

            await this.context.SaveChangesAsync(cancellationToken);
        }

        public async Task<RepairActivity> GetById(Guid id)
        {
            var activity = await this.context.RepairActivities
                .Where(ra => ra.Id == id && !ra.IsDeleted)
                .Include(ra => ra.RepairActivityType)
                .FirstOrDefaultAsync()
                ?? throw new EntityNotFoundException($"Activity not found");

            //var activity = await this.context.RepairActivities
            //    .Include(ra => ra.RepairActivityType)
            //    .FirstOrDefaultAsync(ra => ra.Id == id && !ra.IsDeleted)
            //    ?? throw new EntityNotFoundException($"Activity not found");

            return activity;
        }
    }
}
