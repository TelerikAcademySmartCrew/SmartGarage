using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

namespace SmartGarage.Utilities.Mappers;

public class UserMapper : IUserMapper
{
    public AppUser MaterializeRequestDto(UserRegisterRequestDto userRegisterRequestDto)
    {
        AppUser user = new()
        {
            Email = userRegisterRequestDto.Email,
            UserName = userRegisterRequestDto.Email[..userRegisterRequestDto.Email.IndexOf('@')]
        };
        return user;
    }
}