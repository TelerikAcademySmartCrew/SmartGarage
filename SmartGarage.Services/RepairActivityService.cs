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

		public async Task<ICollection<RepairActivity>> GetByVisitIdAsync(int id)
		{
			return await this.repairActivityRepository.GetByVisitId(id);
		}

		public async Task<ICollection<RepairActivity>> GetByNameAsync(string name)
		{
			return await this.repairActivityRepository.GetByNameAsync(name);
		}

		public async Task<ICollection<RepairActivity>> AddAsync(ICollection<RepairActivity> repairActivities)
		{
			return await this.repairActivityRepository.AddAsync(repairActivities);
		}

		public async Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice)
		{
			return await this.repairActivityRepository.GetByPriceRange(startingPrice, endingPrice);
		}
	}
}
