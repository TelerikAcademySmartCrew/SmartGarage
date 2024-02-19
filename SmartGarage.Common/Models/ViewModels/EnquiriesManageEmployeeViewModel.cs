namespace SmartGarage.Common.Models.ViewModels
{
    public class EnquiriesManageEmployeeViewModel
    {
        public IEnumerable<EnquiryViewModel> Enquiries { get; set; } = new List<EnquiryViewModel>();
    }
}
