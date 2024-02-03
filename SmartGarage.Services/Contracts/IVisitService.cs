using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IVisitService
    {
        Task<ICollection<Visit>> GetByUserIdAsync(string id);
        
        Task<Visit> GetByIdAsync(Guid id);

        Task<Visit> CreateAsync(Visit visit);
    }
}