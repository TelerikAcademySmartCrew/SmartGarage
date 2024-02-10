using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
	public interface IRepairActivityService
	{
		Task<ICollection<RepairActivity>> GetByVisitIdAsync(Guid id);

		Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<RepairActivity> AddAsync(RepairActivity repairActivity);

		Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice);

		Task DeleteAsync(RepairActivity repairActivity);
	
    Task<RepairActivity> GetById(Guid id);
    }
}
