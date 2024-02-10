using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository visitRepository;

        public VisitService(IVisitRepository visitRepository)
        {
            this.visitRepository = visitRepository;
        }
        
        public async Task<ICollection<Visit>> GetAll(VisitsQueryParameters visitsQueryParameters, CancellationToken cancellationToken)
        {
            return await visitRepository.GetAll(visitsQueryParameters, cancellationToken);
        }

        public async Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken)
        {
            return await this.visitRepository.GetByUserIdAsync(id, cancellationToken);
        }

        public async Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.visitRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Visit> CreateAsync(Visit visit, CancellationToken cancellationToken)
        {
            var userVisits = visit.User.Visits.Count + 1;

            if (userVisits >= 27)
            {
                visit.DiscountPercentage = 20;
            }
            else if (userVisits >= 18)
            {
                visit.DiscountPercentage = 15;
            }
            else if (userVisits >= 9)
            {
                visit.DiscountPercentage = 10;
            }

            return await this.visitRepository.CreateAsync(visit, cancellationToken);
        }

        public async Task<Visit> UpdateStatusAsync(Visit visit, CancellationToken cancellationToken)
        {
            return await this.visitRepository.UpdateStatusAsync(visit, cancellationToken);
        }
    }
}