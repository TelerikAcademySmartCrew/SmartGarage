using SmartGarage.Data.Models;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository visitRepository;

        public VisitService(IVisitRepository visitRepository)
        {
            this.visitRepository = visitRepository;
        }
        
        public async Task<ICollection<Visit>> GetByUserIdAsync(string id)
        {
            return await this.visitRepository.GetByUserIdAsync(id);
        }

        public async Task<Visit> GetByIdAsync(int id)
        {
            return await this.visitRepository.GetByIdAsync(id);
        }

        public async Task<Visit> CreateAsync(Visit visit)
        {
            return await this.visitRepository.CreateAsync(visit);
        }
    }
}