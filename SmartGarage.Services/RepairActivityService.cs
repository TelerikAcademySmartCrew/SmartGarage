using SmartGarage.Data.Models;
using SmartGarage.Services.Contracts;
using SmartGarage.Data.Repositories.Contracts;

namespace SmartGarage.Services
{
	public class RepairActivityService : IRepairActivityService
	{
		private readonly IRepairActivityRepository repairActivityRepository;

		public RepairActivityService(IRepairActivityRepository repairActivityRepository)
        {
			this.repairActivityRepository = repairActivityRepository;
		}

		public async Task<ICollection<RepairActivity>> GetByVisitIdAsync(Guid id)
		{
			return await this.repairActivityRepository.GetByVisitId(id);
		}

		public async Task<ICollection<RepairActivity>> GetByNameAsync(string name)
		{
			return await this.repairActivityRepository.GetByNameAsync(name);
		}

		public async Task<RepairActivity> AddAsync(RepairActivity repairActivity)
		{
			return await this.repairActivityRepository.AddAsync(repairActivity);
		}

		public async Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice)
		{
			return await this.repairActivityRepository.GetByPriceRangeAsync(startingPrice, endingPrice);
		}

        public async Task DeleteAsync(RepairActivity repairActivity)
        {
            await this.repairActivityRepository.DeleteAsync(repairActivity);
        }
    }
}
