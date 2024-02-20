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
                var visits = await this.visitService.GetAll(visitQueryParameters, cancellationToken);

                var user = await this.usersService.GetUserAsync(this.User);
                var userViewModel = this.userMapper.Map(user);

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
                var visit = await this.visitService.GetByIdAsync(visitId, cancellationToken);

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
            Guid visitGuid = Guid.Parse(visitId);
            int ratingNumeric = int.Parse(rating);
            try
            {

                var visit = await this.visitService.GetByIdAsync(visitGuid, cancellationToken);

                if (ratingNumeric < 1 || ratingNumeric > 5)
                {
                    throw new InvalidOperationException("Rating must be between 1 and 5");
                }

                visit.Rating = ratingNumeric;
                _ = await this.visitService.UpdateVisitRating(visit, cancellationToken);
                
                return Json(new { success = true, message = "Rating updated successfully" });
            }
            catch (EntityNotFoundException)
            {
                return Json(new { success = false, error = "Visit not found" });
            }
            catch (InvalidOperationException)
            {
                return Json(new { success = false });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(string visitId, CancellationToken cancellationToken)
        {
            try
            {
                var visitGuid = Guid.Parse(visitId);

                var visit = await this.visitService.GetByIdAsync(visitGuid, cancellationToken);

                byte[] pdfBytes = this.pdfGenerator.GeneratePdf(visit);

                var ms = new MemoryStream(pdfBytes);

                return File(ms, "application/pdf", $"Visit-{visit.Vehicle.LicensePlateNumber}-{visit.Date.ToShortDateString()}.pdf");
            }
            catch (Exception)
            {
                return View("DisplayVisit", new { visiid = Guid.Parse(visitId) });
            }
        }
    }
}
