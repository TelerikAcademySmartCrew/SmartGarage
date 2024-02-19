using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository enquiryRepository;

        public EnquiryService(IEnquiryRepository enquiryRepository)
        {
            this.enquiryRepository = enquiryRepository;
        }

        public async Task<Enquiry> CreateAsync(Enquiry enquiry, CancellationToken cancellationToken)
        {
            return await this.enquiryRepository.CreateAsync(enquiry, cancellationToken);
        }

        public async Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters, CancellationToken cancellationToken)
        {
            return await this.enquiryRepository.GetAllAsync(parameters, cancellationToken);
        }

        public async Task<Enquiry> GetById(Guid Id, CancellationToken cancellationToken)
        {
            return await this.enquiryRepository.GetById(Id, cancellationToken);
        }

        public async Task<Enquiry> ReadAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.enquiryRepository.ReadAsync(id, cancellationToken);
        }
    }
}
