using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Services.Contracts
{
    public interface IEnquiryService
    {
        Task<Enquiry> CreateAsync(Enquiry enquiry);

        Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters);

        Task<Enquiry> ReadAsync(Guid id);
    }
}
