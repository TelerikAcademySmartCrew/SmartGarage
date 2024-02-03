using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;

namespace SmartGarage.Utilities.Mappers.Contracts;

public interface IUserMapper
{
    AppUser MaterializeInputModel(UserRegisterInputModel userRegisterInputModel);
}