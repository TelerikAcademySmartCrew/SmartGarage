using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityRepository
    {
		Task<ICollection<RepairActivity>> GetAllAsync(RepairActivityQueryParameters queryParameters);

        Task<ICollection<RepairActivity>> GetByVisitId(int id);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivity);

		Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice);

	}
}
