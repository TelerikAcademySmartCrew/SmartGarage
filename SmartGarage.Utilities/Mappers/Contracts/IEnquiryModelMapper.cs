using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IEnquiryModelMapper
    {
        EnquiryViewModel ToViewModel(Enquiry enquiry);
        IEnumerable<EnquiryViewModel> ToViewModel(IEnumerable<Enquiry> enquiry);
    }
}
