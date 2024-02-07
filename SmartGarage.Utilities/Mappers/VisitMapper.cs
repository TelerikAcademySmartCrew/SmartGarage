using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Data.Models;

namespace SmartGarage.Utilities.Mappers;

public class VisitMapper : IVisitMapper
{
    public List<VisitViewModel> ToViewModel(IEnumerable<Visit> visits)
    {
        return visits.Select(this.ToViewModel).ToList();
    }

    public VisitViewModel ToViewModel(Visit visit)
    {
        return new VisitViewModel
        {
            DateCreated = visit.Date,
            VehicleBrand = visit.Vehicle.Brand.Name,
            VehicleModel = visit.Vehicle.Model.Name,
            RepairActivities = visit.RepairActivities
                .Select(ra => new VisitRepairActivityViewModel
                {
                    Name = ra.RepairActivityType.Name,
                    Price = ra.Price
                })
                .ToList(),
            TotalPrice = visit.RepairActivities.Sum(x => x.Price)
        };
    }

    public Visit MaterializeRequestDto(VisitRequestDto visit, AppUser user, Vehicle vehicle)
    {
        return new Visit
        {
            UserId = user.Id,
            VehicleId = vehicle.Id,
            RepairActivities = visit.RepairActivities
                .Select(x => new RepairActivity
                {
                    RepairActivityType = new RepairActivityType { Name = x.RepairActivityType.Name },
                    Price = x.Price
                })
                .ToList()
        };
    }
}