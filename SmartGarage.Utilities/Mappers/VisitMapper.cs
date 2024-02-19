using SmartGarage.Data.Models;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers
{
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
                Id = visit.Id,
                Status = visit.Status.ToString(),
                Rating = visit.Rating.ToString(),
                DateCreated = visit.Date,
                UserName = visit.User.UserName,
                VehicleBrand = visit.Vehicle.Brand.Name,
                VehicleModel = visit.Vehicle.Model.Name,
                VehicleYear = visit.Vehicle.ProductionYear.ToString(),
                RepairActivities = visit.RepairActivities
                    .Select(x => new VisitRepairActivityViewModel
                    {
                        Id = x.Id.ToString(),
                        Name = x.RepairActivityType.Name,
                        Price = x.Price
                    })
                    .ToList(),
                TotalPrice = visit.RepairActivities.Sum(x => x.Price),
                Discount = visit.DiscountPercentage
            };
        }

        public Visit MaterializeRequestDto(VisitRequestDto visit, string userId, Guid vehicleId)
        {
            return new Visit
            {
                UserId = userId,
                VehicleId = vehicleId,
                Rating = visit.Rating,
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
}