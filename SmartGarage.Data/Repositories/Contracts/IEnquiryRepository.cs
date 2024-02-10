using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IEnquiryRepository
    {
        Task<Enquiry> CreateAsync(Enquiry enquiry);

        Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters);

        Task<Enquiry> ReadAsync(Guid id);
    }
}
