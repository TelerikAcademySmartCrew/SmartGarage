using Microsoft.EntityFrameworkCore;

using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;

namespace SmartGarage.Data.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        private readonly ApplicationDbContext context;

        public VisitRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public async Task<ICollection<Visit>> GetByUserIdAsync(string id)
        {
            return await this.context.Visits
                .Where(x => x.UserId == id)
                .ToListAsync();
        }

        public async Task<Visit> GetByIdAsync(Guid id)
        {
            return await this.context.Visits
                .FirstAsync(x => x.Id == id);
        }

        public async Task<Visit> CreateAsync(Visit visit)
        {
            await this.context.Visits.AddAsync(visit);
            await this.context.SaveChangesAsync();
            return visit;
        }
    }
}