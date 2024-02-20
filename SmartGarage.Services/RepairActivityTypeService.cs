using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
	public class RepairActivityTypeService : IRepairActivityTypeService
	{
		private readonly IRepairActivityTypeRepository repairActivityTypeRepository;

		public RepairActivityTypeService(IRepairActivityTypeRepository repairActivityTypeRepository)
        {
			this.repairActivityTypeRepository = repairActivityTypeRepository;
		}

        public async Task<ICollection<RepairActivityType>> GetAllAsync()
		{
			return await this.repairActivityTypeRepository.GetAllAsync();
		}

		public async Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync()
		{
			return await this.repairActivityTypeRepository.GetAllWithDeletedAsync();
		}

		public async Task<RepairActivityType> CreateAsync(RepairActivityType repairActivityType)
		{
			return await this.repairActivityTypeRepository.CreateAsync(repairActivityType);
		}

		public async Task<RepairActivityType> UpdateAsync(Guid id, string name)
		{
			return await this.repairActivityTypeRepository.UpdateAsync(id, name);
		}

		public async Task DeleteAsync(string name)
		{
			await this.repairActivityTypeRepository.DeleteAsync(name);
		}				
	}
}
