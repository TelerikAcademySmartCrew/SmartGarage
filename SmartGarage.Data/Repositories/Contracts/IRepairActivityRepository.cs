using SmartGarage.Data.Models;

namespace SmartGarage.Data.Repositories.Contracts
{
	public interface IRepairActivityRepository
    {
        Task<ICollection<RepairActivity>> GetByVisitId(int id);

        Task<ICollection<RepairActivity>> GetByNameAsync(string name);

		Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivity);        
    }
}
