using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Data.Models;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVisitMapper
    {
        List<VisitViewModel> ToViewModel(IEnumerable<Visit> visits);

        VisitViewModel ToViewModel(Visit visit);

        Visit MaterializeRequestDto(VisitRequestDto visit, AppUser user, Vehicle vehicle);
    }    
}