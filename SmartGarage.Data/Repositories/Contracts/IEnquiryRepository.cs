using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;

namespace SmartGarage.Data.Repositories.Contracts
{
    public interface IEnquiryRepository
    {
        Task<Enquiry> CreateAsync(Enquiry enquiry, CancellationToken cancellationToken);

        Task<IEnumerable<Enquiry>> GetAllAsync(EnquiryQueryParameters parameters, CancellationToken cancellationToken);

        Task<Enquiry> GetById(Guid Id, CancellationToken cancellationToken);

        Task<Enquiry> ReadAsync(Guid id, CancellationToken cancellationToken);
    }
}
