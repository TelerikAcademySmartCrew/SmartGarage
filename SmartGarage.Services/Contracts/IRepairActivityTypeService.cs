using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
	public interface IRepairActivityTypeService
	{
		Task<ICollection<RepairActivityType>> GetAllAsync();

		Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync();

		Task<RepairActivityType> UpdateAsync(Guid id, string name);

		Task<RepairActivityType> CreateAsync(RepairActivityType repairActivityType);

		Task DeleteAsync(string name);
	}
}
