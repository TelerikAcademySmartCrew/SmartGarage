using SmartGarage.Data.Models;
using SmartGarage.Utilities.Models;
using SmartGarage.Utilities.Models.ViewModels;

namespace SmartGarage.Utilities.Mappers.Contracts
{
    public interface IVisitMapper
    {
        List<VisitViewModel> ToViewModel(IEnumerable<Visit> visits);

        VisitViewModel ToViewModel(Visit visit);
    }    
}