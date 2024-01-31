using SmartGarage.Data.Models.DTOs;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services.Mappers.Contracts;

public interface IUserMapper
{
    AppUser Map(UserRegisterDTO userRegisterDto);
}