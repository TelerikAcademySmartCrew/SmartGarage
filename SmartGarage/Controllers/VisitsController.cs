using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Controllers
{
    public class VisitsController : BaseCustomerController
    {
        private readonly IUsersService usersService;
        private readonly IVisitService visitService;
        private readonly IVisitMapper visitMapper;
        private readonly IUserMapper userMapper;
        private readonly PDFGenerator pdfGenerator;

        public VisitsController(IUsersService usersService,
            IVisitService visitService,
            IVisitMapper visitMapper,
            IUserMapper userMapper,
            PDFGenerator pdfGenerator)
        {
            this.usersService = usersService;
            this.visitService = visitService;
            this.visitMapper = visitMapper;
            this.userMapper = userMapper;
            this.pdfGenerator = pdfGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAll(CancellationToken cancellationToken)
        {
            try
            {
                var visitQueryParameters = new VisitsQueryParameters();
                var visits = await visitService.GetAll(visitQueryParameters, cancellationToken);

                var user = await usersService.GetUserAsync(User);
                var userViewModel = userMapper.Map(user);

                return View(userViewModel);
            }
            catch (EntityNotFoundException)
            {
                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DisplayVisit(Guid visitId, CancellationToken cancellationToken)
        {
            try
            {
                var visit = await visitService.GetByIdAsync(visitId, cancellationToken);

                var visitViewModel = this.visitMapper.ToViewModel(visit);

                return View(visitViewModel);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVisitRating(string visitId, string rating, CancellationToken cancellationToken)
        {
            Guid _visitId = Guid.Parse(visitId);
            int _rating = int.Parse(rating);
            try
            {

                var visit = await visitService.GetByIdAsync(_visitId, cancellationToken);

                if (_rating < 1 || _rating > 5)
                {
                    throw new InvalidOperationException("Rating must be between 1 and 5");
                }

                visit.Rating = _rating;
                _ = await visitService.UpdateVisitRating(visit, cancellationToken);
                return RedirectToAction("DisplayAll", "Visits");
            }
            catch (EntityNotFoundException)
            {
                return View("DisplayVisit", _visitId);
            }
            catch (InvalidOperationException)
            {
                return View("DisplayVisit", _visitId);
            }
            catch (Exception)
            {
                return RedirectToAction("DisplayAll", "Visits");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(string visitId, CancellationToken cancellationToken)
        {
            try
            {
                var _visiitId = Guid.Parse(visitId);

                var visit = await visitService.GetByIdAsync(_visiitId, cancellationToken);

                // Your byte array (replace this with your actual byte array)
                byte[] pdfBytes = pdfGenerator.GeneratePdf(visit);

                // Convert byte array to MemoryStream
                var ms = new MemoryStream(pdfBytes);

                // Return the PDF as a file for download
                //return File(ms, "application/pdf", "Visit-" + visit.Vehicle.LicensePlateNumber + + ".pdf");
                return File(ms, "application/pdf", $"Visit-{visit.Vehicle.LicensePlateNumber}-{visit.Date.ToShortDateString()}.pdf");
            }
            catch (Exception ex)
            {
                return View("DisplayVisit", new { visiid = Guid.Parse(visitId) });
            }
        }
    }
}
