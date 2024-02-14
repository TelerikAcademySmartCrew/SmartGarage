using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IRepairActivityService
    {
        Task<ICollection<RepairActivity>> GetByVisitIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name, CancellationToken cancellationToken);

        Task<RepairActivity> GetById(Guid id);

        Task<RepairActivity> AddAsync(RepairActivity repairActivity, CancellationToken cancellationToken);

        Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
