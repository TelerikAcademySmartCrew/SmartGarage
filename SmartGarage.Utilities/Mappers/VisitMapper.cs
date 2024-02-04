using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

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
}