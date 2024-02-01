using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityTypeRepository
    {
        Task<ICollection<RepairActivityType>> GetAllAsync();

        Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync();
    }
}
