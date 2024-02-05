using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityRepository
    {
		Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters);

        Task<ICollection<RepairActivity>> GetByVisitId(Guid id);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivity);

		Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice);

	}
}
