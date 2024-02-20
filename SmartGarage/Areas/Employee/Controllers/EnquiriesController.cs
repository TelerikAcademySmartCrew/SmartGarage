using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Contract;
using System.Text;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class EnquiriesController : BaseEmployeeController
    {
        private readonly IEnquiryService enquiryService;
        private readonly IEnquiryModelMapper enquiryModelMapper;
        private readonly IEmailService emailService;

        public EnquiriesController(IEnquiryService enquiryService,
            IEnquiryModelMapper enquiryModelMapper,
            IEmailService emailService)
        {
            this.enquiryService = enquiryService;
            this.enquiryModelMapper = enquiryModelMapper;
            this.emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeGetAllEnquiries(CancellationToken cancellationToken)
        {
            base.InitializeUserName();

            var enquiryQueryParameters = new EnquiryQueryParameters
            {
                IsRead = false,
            };

            var enquiriesManageEmployeeViewModel = await this.enquiryService.GetAllAsync(enquiryQueryParameters, cancellationToken);

            var viewModel = new EnquiriesManageEmployeeViewModel
            {
                Enquiries = this.enquiryModelMapper.ToViewModel(enquiriesManageEmployeeViewModel)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostEnquiry(EnquiryReplyViewModel enquiryViewModel, CancellationToken cancellationToken)
        {
            try
            {
                var enquiryId = Guid.Parse(enquiryViewModel.Id);
                var enquiry = await this.enquiryService.GetById(enquiryId, cancellationToken);

                const string subject = "Helo from Smart Garage!";

                var sb = new StringBuilder();

                sb.AppendLine("To: " + enquiryViewModel.Email);
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine(enquiryViewModel.Content);
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine(enquiryViewModel.Reply);

                string body = sb.ToString();

                var toEmail = enquiryViewModel.Email;

                await this.enquiryService.ReadAsync(enquiryId, cancellationToken);

                await this.emailService.SendEmailAsync(toEmail, subject, body);

                return Json(new { success = true, message = sb });
            }
            catch (DuplicateEntityFoundException)
            {
                return Json(new { success = true, message = "Enquiry ID not found." });
            }
        }
    }
}
