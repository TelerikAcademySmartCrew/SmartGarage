using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityRepository
    {
		Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters);

        Task<ICollection<RepairActivity>> GetByVisitId(Guid id);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<RepairActivity> AddAsync(RepairActivity repairActivity);

		Task<ICollection<RepairActivity>> GetByPriceRangeAsync(int startingPrice, int endingPrice);

		Task DeleteAsync(RepairActivity repairActivity);
	}
}
