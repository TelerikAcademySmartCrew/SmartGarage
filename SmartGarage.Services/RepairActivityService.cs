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

		public async Task<ICollection<RepairActivity>> GetByVisitIdAsync(Guid id, CancellationToken cancellationToken)
		{
			return await this.repairActivityRepository.GetByVisitId(id, cancellationToken);
		}

		public async Task<ICollection<RepairActivity>> GetByNameAsync(string name, CancellationToken cancellationToken)
		{
			return await this.repairActivityRepository.GetByNameAsync(name, cancellationToken);
		}

		public async Task<RepairActivity> AddAsync(RepairActivity repairActivity, CancellationToken cancellationToken)
		{
			return await this.repairActivityRepository.AddAsync(repairActivity, cancellationToken);
		}

		public async Task<ICollection<RepairActivity>> GetByPriceRange(int startingPrice, int endingPrice, CancellationToken cancellationToken)
		{
			return await this.repairActivityRepository.GetByPriceRangeAsync(startingPrice, endingPrice, cancellationToken);
		}

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await this.repairActivityRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<RepairActivity> GetById(Guid id)
        {
            return await this.repairActivityRepository.GetById(id);
        }
    }
}
