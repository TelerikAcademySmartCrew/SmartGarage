using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVisitRepository
    {
        Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken);
        
        Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Visit> CreateAsync(Visit visit, CancellationToken cancellationToken);
    }
}