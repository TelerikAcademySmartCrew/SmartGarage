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
				.Where(x => !x.IsDeleted)
				.ToListAsync();
		}

		public async Task<ICollection<RepairActivityType>> GetAllWithDeletedAsync()
		{
			return await this.context.RepairActivityTypes
				.ToListAsync();
		}

		public async Task<RepairActivityType> CreateAsync(RepairActivityType repairActivityType)
		{
			if (await this.RepairActivityTypeExists(repairActivityType.Name))
			{
				throw new EntityAlreadyExistsException(TypeAlreadyExists);
			}

			await this.context.RepairActivityTypes.AddAsync(repairActivityType);
			await this.context.SaveChangesAsync();

			return repairActivityType;
		}

		public async Task<RepairActivityType> UpdateAsync(string name)
		{
			var repairActivityType = await this.context.RepairActivityTypes
				.FirstAsync(x => x.Name == name);

			repairActivityType.Name = name;

			await this.context.SaveChangesAsync();

			return repairActivityType;
		}

		public async Task DeleteAsync(string name)
		{
			if (!await this.RepairActivityTypeExists(name))
			{
				throw new EntityNotFoundException(TypeNotFound);
			}

			var repairActivityType = await this.context.RepairActivityTypes
				.FirstAsync(x => x.Name == name);

            repairActivityType.IsDeleted = true;

			await this.context.SaveChangesAsync();
		}
		
		private async Task<bool> RepairActivityTypeExists(string name)
		{
			return await this.context.RepairActivityTypes
				.AnyAsync(x => x.Name == name);
		}
	}
}
