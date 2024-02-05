using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Data.Models;
using SmartGarage.Utilities.Mappers.Contracts;
using SmartGarage.Utilities.Models;

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