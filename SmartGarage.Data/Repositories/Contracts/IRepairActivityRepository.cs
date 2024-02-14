using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IRepairActivityRepository
    {
        Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters, CancellationToken cancellationToken);

        Task<RepairActivity> GetById(Guid id);

        Task<ICollection<RepairActivity>> GetByVisitId(Guid id, CancellationToken cancellationToken);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name, CancellationToken cancellationToken);

        Task<RepairActivity> AddAsync(RepairActivity repairActivity, CancellationToken cancellationToken);

        Task<ICollection<RepairActivity>> GetByPriceRangeAsync(int startingPrice, int endingPrice, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}