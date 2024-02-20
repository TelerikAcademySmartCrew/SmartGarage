using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVisitRepository
    {
        Task<ICollection<Visit>> GetAll(VisitsQueryParameters visitsQueryParameters, CancellationToken cancellationToken);
        
        Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken);
        
        Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Visit> CreateAsync(Visit visit, CancellationToken cancellationToken);

        Task<Visit> UpdateStatusAsync(Visit visit, CancellationToken cancellationToken);
        
        Task<Visit> UpdateVisitRating(Visit visit, CancellationToken cancellationToken);

        Task<Visit> UpdateVisitRepairActivities(Visit visit, ICollection<VisitRepairActivityViewModel> repairActivities, CancellationToken cancellationToken);
    }
}