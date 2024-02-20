namespace SmartGarage.Common.Models.ViewModels
{
    public class EnquiryViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Content { get; set; } = null!;

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ReadableDate => DateCreated.ToString("yyyy-MM-dd HH:mm:ss");

        public string FormattedPhoneNumber => FormatPhoneNumber(PhoneNumber);

        private string FormatPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length == 10)
            {
                return string.Format("{0:(####) ###-###}", int.Parse(phoneNumber));
            }

            return phoneNumber;
        }
    }
}
