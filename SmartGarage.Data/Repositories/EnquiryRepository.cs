using Microsoft.EntityFrameworkCore;

using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Enquiry;

namespace SmartGarage.Data.Repositories
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly ApplicationDbContext context;

        public EnquiryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Enquiry> CreateAsync(Enquiry enquiry, CancellationToken cancellationToken)
        {
            await this.context.Enquiries.AddAsync(enquiry);
            await this.context.SaveChangesAsync();

            return enquiry;
        }

        public async Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters, CancellationToken cancellationToken)
        {
            var enquiries = this.context.Enquiries.AsQueryable();

            if (parameters.IsRead == true)
            {
                enquiries = enquiries.Where(x => x.IsRead);
            }
            else if (parameters.IsRead == false)
            {
                enquiries = enquiries.Where(x => !x.IsRead);
            }

            return await enquiries.ToListAsync();
        }

        public async Task<Enquiry> GetById(Guid Id, CancellationToken cancellationToken)
        {
            var enquiry = await this.context.Enquiries
                .FirstOrDefaultAsync(x => x.Id == Id)
                ?? throw new EntityNotFoundException(EnquiryNotFound);

            return enquiry;
        }

        public async Task<Enquiry> ReadAsync(Guid id, CancellationToken cancellationToken)
        {
            var enquiry = await this.context.Enquiries
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new EntityNotFoundException(EnquiryNotFound);

            enquiry.IsRead = true;
            
            await this.context.SaveChangesAsync();

            return enquiry;
        }
    }
}
