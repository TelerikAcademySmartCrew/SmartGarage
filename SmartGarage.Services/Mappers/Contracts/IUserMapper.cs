using SmartGarage.Data.Models.DTOs;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Mappers.Contracts;

public interface IUserMapper
{
    AppUser Map(UserRegisterDTO userRegisterDto);
}