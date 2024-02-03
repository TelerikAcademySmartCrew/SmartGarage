using SmartGarage.Data.Models;
using SmartGarage.Data.Models.DTOs;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers;

public class UserMapper : IUserMapper
{
    public AppUser MaterializeInputModel(UserRegisterInputModel userRegisterInputModel)
    {
        AppUser user = new()
        {
            Email = userRegisterInputModel.Email,
            UserName = userRegisterInputModel.Email[..userRegisterInputModel.Email.IndexOf('@')]
        };
        return user;
    }
}