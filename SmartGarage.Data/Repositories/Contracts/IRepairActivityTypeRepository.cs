using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityTypeRepository
    {
        Task<ICollection<RepairActivityType>> GetAllAsync();

        Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync();

        Task<RepairActivityType> UpdateAsync(string name);

        Task<RepairActivityType> CreateAsync(RepairActivityType repairActivityType);

        Task DeleteAsync(string name);
    }
}
