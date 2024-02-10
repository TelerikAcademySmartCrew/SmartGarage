using Microsoft.AspNetCore.Mvc;
using SmartGarage.Common.Exceptions;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data;
using SmartGarage.Data.Models;
using SmartGarage.Data.Models.QueryParameters;
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

                var createdVisit = await visitService.CreateAsync(newVisit, cancellationToken);

                //return View("DisplayVisitDetails", new { id = createdVisit.Id });
                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = newVisit.Id });
            }
            catch (EntityNotFoundException ex)
            {
                this.ModelState.AddModelError("All", ex.Message);
                return View(vehicleNumbers);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageVisits([FromQuery] VisitsQueryParameters visitsQueryParameters, CancellationToken cancellationToken)
        {
            InitializeUserName();

            try
            {
                if (visitsQueryParameters == null)
                {
                    visitsQueryParameters = new VisitsQueryParameters();
                }

                var allVisits = await visitService.GetAll(visitsQueryParameters, cancellationToken);

                var visitsViewModel = this.visitMapper.ToViewModel(allVisits);

                //foreach (var visit in allVisits)
                //{
                //    visitsViewModel.Add(new VisitViewModel
                //    {
                //        Id = visit.Id,
                //        DateCreated = visit.Date,
                //        UserName = visit.User.UserName,
                //        VehicleBrand = visit.Vehicle.Brand.Name,
                //        VehicleModel = visit.Vehicle.Model.Name,
                //        TotalPrice = visit.RepairActivities.Sum(visit => visit.Price),
                //        RepairActivities = new List<VisitRepairActivityViewModel>()
                //        {
                //            new VisitRepairActivityViewModel
                //            {
                //                Name = "Oil chagne",
                //                Price = 30.0
                //            },
                //            new VisitRepairActivityViewModel
                //            {
                //                Name = "Air filter chagne",
                //                Price = 20.0
                //            }
                //        }
                //    });
                //}

                return View(visitsViewModel);
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
                    ModelState.AddModelError("Any", "Repair activity type already added.");

                    return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
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

                await this.repairActivityService.AddAsync(new List<RepairActivity>(){
                    newRepairActivity
                });

                visit.RepairActivities.Add(newRepairActivity);

                this.applicationDbContext.SaveChanges();

                var visitViewModel = visitMapper.ToViewModel(visit);

                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
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
                Guid _id = Guid.Parse(visitId);
                var visit = await this.visitService.GetByIdAsync(_id, cancellationToken);

                Guid _visitRepairActivityId = Guid.Parse(visitRepairActivityId);
                var repairActivity = await this.repairActivityService.GetById(_visitRepairActivityId);

                visit.RepairActivities.Remove(repairActivity);

                this.applicationDbContext.SaveChanges();

                //var visitViewModel = visitMapper.ToViewModel(visit);

                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("DisplayVisitDetails", "Visits", new { area = "Employee", visitId = visitId });
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
    }
}
