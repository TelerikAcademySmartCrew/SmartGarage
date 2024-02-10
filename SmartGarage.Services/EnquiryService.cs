using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Services.Contracts;

namespace SmartGarage.Services
{
    internal class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository enquiryRepository;

        public EnquiryService(IEnquiryRepository enquiryRepository)
        {
            this.enquiryRepository = enquiryRepository;
        }

        public async Task<Enquiry> CreateAsync(Enquiry enquiry)
        {
            return await this.enquiryRepository.CreateAsync(enquiry);
        }

        public async Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters)
        {
            return await this.enquiryRepository.GetAllAsync(parameters);
        }

        public async Task<Enquiry> ReadAsync(Guid id)
        {
            return await this.enquiryRepository.ReadAsync(id);
        }
    }
}
