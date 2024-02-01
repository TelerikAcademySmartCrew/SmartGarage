using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.RepairActivity;

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

		public async Task<RepairActivityType> CreateAsync(RepairActivityType repairActivityType)
		{
			var isExistent = await this.RepairActivityTypeExists(repairActivityType.Name);

			if (isExistent)
			{
				throw new EntityAlreadyExistsException(AlreadyExists);
			}

			await this.context.RepairActivityTypes.AddAsync(repairActivityType);
			await this.context.SaveChangesAsync();
			return repairActivityType;
		}

		public async Task<RepairActivityType> UpdateAsync(string name)
		{
			var rat = await this.context.RepairActivityTypes
				.FirstAsync(rat => rat.Name == name);

			rat.Name = name;
			await this.context.SaveChangesAsync();
			return rat;
		}

		public async Task DeleteAsync(string name)
		{
			var rat = await this.context.RepairActivityTypes
				.FirstAsync(rat => rat.Name == name);

			rat.IsDeleted = true;
			await this.context.SaveChangesAsync();
		}
		
		private async Task<bool> RepairActivityTypeExists(string name)
		{
			return await this.context.RepairActivityTypes
				.AnyAsync(rat => rat.Name == name);
		}
	}
}
