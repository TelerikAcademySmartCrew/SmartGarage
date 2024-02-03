using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers;

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