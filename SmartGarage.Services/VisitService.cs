using Microsoft.AspNetCore.Hosting;

using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Common.Enumerations;
using SmartGarage.Services.Contracts;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Data.Repositories.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Contract;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Status;

namespace SmartGarage.Services
{
    public class VisitService : IVisitService
    {
        private readonly IVisitRepository visitRepository;
        private readonly IEmailService emailService;
        private readonly PDFGenerator pdfGenerator;
        private readonly IWebHostEnvironment webHostEnvironment;

        public VisitService(IVisitRepository visitRepository, IEmailService emailService, PDFGenerator pdfGenerator, IWebHostEnvironment webHostEnvironment)
        {
            this.visitRepository = visitRepository;
            this.emailService = emailService;
            this.pdfGenerator = pdfGenerator;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<ICollection<Visit>> GetAll(VisitsQueryParameters visitsQueryParameters, CancellationToken cancellationToken)
        {
            return await this.visitRepository.GetAll(visitsQueryParameters, cancellationToken);
        }

        public async Task<ICollection<Visit>> GetByUserIdAsync(string id, CancellationToken cancellationToken)
        {
            return await this.visitRepository.GetByUserIdAsync(id, cancellationToken);
        }

        public async Task<Visit> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.visitRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<Visit> CreateAsync(Visit visit, int vehicleVisitsCount, CancellationToken cancellationToken)
        {
            var userVisits = vehicleVisitsCount + 1;

            if (userVisits >= 27)
            {
                visit.DiscountPercentage = 20;
            }
            else if (userVisits >= 18)
            {
                visit.DiscountPercentage = 15;
            }
            else if (userVisits >= 9)
            {
                visit.DiscountPercentage = 10;
            }

            return await this.visitRepository.CreateAsync(visit, cancellationToken);
        }

        public async Task<Visit> UpdateStatusAsync(Visit visit, CancellationToken cancellationToken)
        {
            if (visit.Status == Status.Paid)
            {
                throw new InvalidOperationException(CannotUpdateStatus);
            }

            visit.Status++;

            var updatedVist = await this.visitRepository.UpdateStatusAsync(visit, cancellationToken);

            if (visit.Status == Status.Paid)
            {
                const string subject = "Repair complete!";

                // Get the wwwroot path
                var wwwrootPath = this.webHostEnvironment.WebRootPath;

                var filePath = Path.Combine(wwwrootPath, "PaidVisitInvoice.html")
                    ?? throw new EntityNotFoundException("Email template not found.");

                var body = string.Empty;

                using (var reader = new StreamReader(filePath))
                {
                    body = await reader.ReadToEndAsync();
                }

                body = body.Replace("{UserName}", visit.User.Email);

                var pdfDocument = this.pdfGenerator.GeneratePdf(visit);

                _ = this.emailService.SendEmailAsync(visit.User.Email, subject, body, pdfDocument);
            }

            return updatedVist;
        }

        public async Task<Visit> UpdateVisitRating(Visit visit, CancellationToken cancellationToken)
        {
            return await this.visitRepository.UpdateVisitRating(visit, cancellationToken);
        }

        public async Task<Visit> UpdateVisitRepairActivities(Visit visit, ICollection<VisitRepairActivityViewModel> repairActivities, CancellationToken cancellationToken)
        {
            return await this.visitRepository.UpdateVisitRepairActivities(visit, repairActivities, cancellationToken);
        }
    }
}