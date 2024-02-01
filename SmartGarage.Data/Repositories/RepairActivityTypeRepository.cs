using Microsoft.EntityFrameworkCore;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;

namespace SmartGarage.Data.Repositories
{
	public class RepairActivityTypeRepository : IRepairActivityTypeRepository
	{
		private readonly ApplicationDbContext context;

		public RepairActivityTypeRepository(ApplicationDbContext context)
        {
			this.context = context;
		}

        public async Task<ICollection<RepairActivityType>> GetAllAsync()
		{
			return await this.context.RepairActivityTypes
				.Where(rat => !rat.IsDeleted)
				.ToListAsync();
		}

		public async Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync()
		{
			return await this.context.RepairActivityTypes
				.ToListAsync();
		}
	}
}
