using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
	public interface IRepairActivityService
	{
		Task<ICollection<RepairActivity>> GetByVisitIdAsync(int id);

		Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<ICollection<RepairActivity>> AddAsync (ICollection<RepairActivity> repairActivities);

		Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice);
	}
}
