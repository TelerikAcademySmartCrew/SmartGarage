using SmartGarage.Data.Models.DTOs;
using SmartGarage.Services.Mappers.Contracts;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Mappers;

public class UserMapper : IUserMapper
{
    public AppUser Map(UserRegisterDTO userRegisterDto)
    {
        AppUser user = new()
        {
            Email = userRegisterDto.Email,
            UserName = userRegisterDto.Email[..userRegisterDto.Email.IndexOf('@')]
        };
        return user;
    }
}