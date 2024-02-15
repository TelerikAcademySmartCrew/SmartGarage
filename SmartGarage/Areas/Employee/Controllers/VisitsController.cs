using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartGarage.Common.Enumerations;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
using SmartGarage.Services;
using SmartGarage.Services.Contracts;
using SmartGarage.Utilities.Mappers;
using SmartGarage.Utilities.Mappers.Contracts;
using static SmartGarage.Common.Exceptions.ExceptionMessages.Vehicle;

namespace SmartGarage.Areas.Employee.Controllers
{
    public class VisitsController : BaseEmployeeController
    {
        private readonly IVisitService visitService;
        private readonly IVehicleService vehicleService;
        private readonly IRepairActivityService repairActivityService;
        private readonly IRepairActivityTypeService repairActivityTypeService;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IVisitMapper visitMapper;

        public VisitsController(IVisitService visitService,
            IVehicleService vehicleService,
            IRepairActivityService repairActivityService,
            IRepairActivityTypeService repairActivityTypeService,
            ApplicationDbContext applicationDbContext,
            IVisitMapper visitMapper)
        {
            this.visitService = visitService;
            this.vehicleService = vehicleService;
            this.repairActivityService = repairActivityService;
            this.repairActivityTypeService = repairActivityTypeService;
            this.applicationDbContext = applicationDbContext;
            this.visitMapper = visitMapper;
        }

        [HttpGet]
        public async Task<IActionResult> CreateVisit(CancellationToken cancellationToken)
        {
            base.InitializeUserName();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisit(VisitCreateViewModel vehicleNumbers, CancellationToken cancellationToken)
        {
            base.InitializeUserName();

            try
            {
                if (!ModelState.IsValid)
                {
                    if (vehicleNumbers.VIN != null)
                        ModelState.AddModelError("VIN", "Incorrect VIN");
                    if (vehicleNumbers.LicensePlateNumber != null)
                        ModelState.AddModelError("LicensePlateNumber", "Incorrect LPN");
                }

                var vehicleQueryParameters = new VehicleQueryParameters()
                {
                    VIN = vehicleNumbers.VIN,
                    LicensePlate = vehicleNumbers.LicensePlateNumber
                };

                var allVehicles = await vehicleService.GetAllAsync(vehicleQueryParameters, cancellationToken);
                var vehicle = allVehicles.FirstOrDefault() ?? throw new EntityNotFoundException(VehicleNotFound);

                var newVisit = new Visit()
                {
                    Date = DateTime.Now,
                    UserId = vehicle.UserId,
                    VehicleId = vehicle.Id,
                };

                var createdVisit = await visitService.CreateAsync(newVisit, vehicle.Visits.Count, cancellationToken);

                //return View("DisplayVisitDetails", new { id = createdVisit.Id });
                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = newVisit.Id });
            }
            catch (EntityNotFoundException ex)
            {
                if (vehicleNumbers.VIN != null)
                    ModelState.AddModelError("VIN", ex.Message);
                if (vehicleNumbers.LicensePlateNumber != null)
                    ModelState.AddModelError("LicensePlateNumber", ex.Message);
                return View(vehicleNumbers);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageVisits(VisitsQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            InitializeUserName();

            try
            {
                var allVisits = await this.visitService.GetAll(queryParameters, cancellationToken);

                var visitsViewModel = this.visitMapper.ToViewModel(allVisits);

                var visitsModel = new ManageVisitsViewModel
                {
                    Visits = visitsViewModel,
                    VisitsQueryParameters = queryParameters
                };

                return View(visitsModel);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddRepairActivity(string visitId,
            string repairActivityTypeId,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid _visitId = Guid.Parse(visitId);
                Guid _repairActivityTypeId = Guid.Parse(repairActivityTypeId);
                var visit = await this.visitService.GetByIdAsync(_visitId, cancellationToken);

                if (visit.RepairActivities.Any(activity => activity.RepairActivityTypeId == _repairActivityTypeId))
                {
                    this.ModelState.AddModelError("Any", "Repair activity type already added.");
                    var viewModelToReturn = this.visitMapper.ToViewModel(visit);
                    //return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
                    return PartialView("~/Views/Shared/Visits/_VisitDetailsEditablePartial.cshtml", viewModelToReturn);
                }

                // TODO : check if there's already activity of type added

                var allRepairActivityTypes = await this.repairActivityTypeService.GetAllAsync();
                var repairActivityType = allRepairActivityTypes.FirstOrDefault(a => a.Id == _repairActivityTypeId)
                    ?? throw new EntityNotFoundException("Repair activity type not found");

                var newRepairActivity = new RepairActivity
                {
                    Id = Guid.NewGuid(),
                    RepairActivityTypeId = _repairActivityTypeId,
                    RepairActivityType = repairActivityType,
                    VisitId = visit.Id,
                    Visit = visit
                };

                await this.repairActivityService.AddAsync(newRepairActivity, cancellationToken);

                this.applicationDbContext.SaveChanges();

                var visitViewModel = this.visitMapper.ToViewModel(visit);

                //return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
                return PartialView("~/Views/Shared/Visits/_VisitDetailsEditablePartial.cshtml", visitViewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRepairActivity(string visitId,
            string visitRepairActivityId,
            CancellationToken cancellationToken)
        {
            try
            {
                Guid _visitRepairActivityId = Guid.Parse(visitRepairActivityId);
                //var repairActivity = await this.repairActivityService.GetById(_visitRepairActivityId, cancellationToken);

                await this.repairActivityService.DeleteAsync(_visitRepairActivityId, cancellationToken);

                Guid _id = Guid.Parse(visitId);
                var visit = await this.visitService.GetByIdAsync(_id, cancellationToken);

                var visitViewModel = this.visitMapper.ToViewModel(visit);

                return PartialView("~/Views/Shared/Visits/_VisitDetailsEditablePartial.cshtml", visitViewModel);
                //return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
            }
            catch (EntityNotFoundException ex)
            {
                //return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
                return View("DisplayVisitDetails", new { area = "Employee", visitId = visitId });
            }
        }

        [HttpGet]
        public IActionResult ShowActiveVisits()
        {
            base.InitializeUserName();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DisplayVisitDetails(string visitId, CancellationToken cancellationToken)
        {
            base.InitializeUserName();

            try
            {
                var guid = Guid.Parse(visitId);
                var visit = await this.visitService.GetByIdAsync(guid, cancellationToken);

                var visitsViewModel = this.visitMapper.ToViewModel(visit);

                var allVisitRepairTypes = await this.repairActivityTypeService.GetAllAsync();

                var allVisitRepairTypesViewModel = new List<VisitRepairActivityCreateViewModel>();
                foreach (var item in allVisitRepairTypes)
                {
                    allVisitRepairTypesViewModel.Add(new VisitRepairActivityCreateViewModel()
                    {
                        Id = item.Id.ToString(),
                        Name = item.Name,
                    });
                }

                visitsViewModel.RepairActivityTypes = allVisitRepairTypesViewModel;

                return View(visitsViewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVisitStatus(string visitId, CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(visitId);

            try
            {
                var visit = await visitService.GetByIdAsync(id, cancellationToken);

                await visitService.UpdateStatusAsync(visit, cancellationToken);

                return RedirectToAction("DisplayVisitDetails", new { area = "Employee", visitId = visitId });
            }
            catch (EntityNotFoundException ex)
            {
                return RedirectToAction("DisplayVisitDetails", new { area = "Employee", visitId = visitId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVisit(string visitId, List<VisitRepairActivityViewModel> repairActivities, CancellationToken cancellationToken)
        {
            Guid id = Guid.Parse(visitId);
            var visit = await visitService.GetByIdAsync(id, cancellationToken);


            foreach (var item in repairActivities)
            {
                Guid repairActivityId = Guid.Parse(item.Id);
                var entry = visit.RepairActivities.First(x => x.Id == repairActivityId);
                entry.Price = item.Price;
            }

            applicationDbContext.SaveChanges();

            // To update the visit data using visitsService
            return RedirectToAction("DisplayVisitDetails", new { area = "Employee", visitId = visit.Id });
        }
    }
}
