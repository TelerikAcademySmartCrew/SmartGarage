using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVisitMapper
    {
        List<VisitViewModel> ToViewModel(IEnumerable<Visit> visits);

        VisitViewModel ToViewModel(Visit visit);

        Visit MaterializeRequestDto(VisitRequestDto visit, string userId, Guid vehicleId);
    }    
}