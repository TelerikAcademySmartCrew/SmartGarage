using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IVisitService
    {
        Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken);
        
        Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Visit> CreateAsync(Visit visit, CancellationToken cancellationToken);
    }
}