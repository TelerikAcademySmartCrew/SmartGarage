using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IVisitRepository
    {
        Task<ICollection<Visit>> GetByUserIdAsync(string id);
        
        Task<Visit> GetByIdAsync(int id);

        Task<Visit> CreateAsync(Visit visit);
    }
}