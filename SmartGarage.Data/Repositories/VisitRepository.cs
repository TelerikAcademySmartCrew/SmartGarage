using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Common.Enumerations;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Visit;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Status;

namespace SmartGarage.Data.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly ApplicationDbContext context;

        public VisitRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Visit>> GetAll(VisitsQueryParameters visitsQueryParameters, CancellationToken cancellationToken)
        {
            return await context.Visits.Select(visit => visit)
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Brand)
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(v => v.RepairActivities)
                    .ThenInclude(v => v.RepairActivityType)
                .Include(v => v.User).ToListAsync();
        }

        public async Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken)
        {
            return await this.context.Visits
                .Where(x => x.UserId == id)
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Brand)
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(v => v.RepairActivities)
                    .ThenInclude(v => v.RepairActivityType)
                .Include(v => v.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.context.Visits
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Brand)
                .Include(v => v.Vehicle)
                    .ThenInclude(v => v.Model)
                .Include(v => v.RepairActivities)
                    .ThenInclude(v => v.RepairActivityType)
                .Include(v => v.User)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
                ?? throw new EntityNotFoundException(VisitNotFound);
        }

        public async Task<Visit> CreateAsync(Visit visit, CancellationToken cancellationToken)
        {
            await this.context.Visits.AddAsync(visit, cancellationToken);

            foreach (var repairActivity in visit.RepairActivities)
            {
                repairActivity.VisitId = visit.Id;
            }

            await this.context.RepairActivities.AddRangeAsync(visit.RepairActivities);
            await this.context.SaveChangesAsync(cancellationToken);
            return visit;
        }

        public async Task<Visit> UpdateStatusAsync(Visit visit, CancellationToken cancellationToken)
        {
            if (visit.Status == Status.Paid)
            {
                throw new InvalidOperationException(CannotUpdateStatus);
            }

            visit.Status++;
            await this.context.SaveChangesAsync();
            return visit;
        }
    }
}