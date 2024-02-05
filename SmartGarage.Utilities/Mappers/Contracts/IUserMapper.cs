using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Utilities.Mappers.Contracts;

public interface IUserMapper
{
    AppUser MaterializeInputModel(UserRegisterInputModel userRegisterInputModel);
}