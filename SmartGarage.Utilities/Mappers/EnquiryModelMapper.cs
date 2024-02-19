using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers
{
    public class EnquiryModelMapper : IEnquiryModelMapper
    {
        public EnquiryViewModel ToViewModel(Enquiry enquiry)
        {
            var viewModel = new EnquiryViewModel
            {
                Id = enquiry.Id.ToString(),
                Email = enquiry.Email,
                PhoneNumber = enquiry.PhoneNumber,
                Content = enquiry.Content,
                IsRead = enquiry.IsRead,
                IsDeleted = enquiry.IsDeleted,
                DateCreated = enquiry.DateCreated,
            };

            return viewModel;
        }

        public IEnumerable<EnquiryViewModel> ToViewModel(IEnumerable<Enquiry> enquiry)
        {
            var viewModel = new List<EnquiryViewModel>();

            foreach (var item in enquiry)
            {
                viewModel.Add(ToViewModel(item));
            }

            return viewModel;
        }
    }
}
