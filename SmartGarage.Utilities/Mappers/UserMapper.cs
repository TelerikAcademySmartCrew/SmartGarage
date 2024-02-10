using SmartGarage.Common.Models;
using SmartGarage.Common.Models.InputModels;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;
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

    public UserViewModel Map(AppUser user)
    {
        var newUserViewModel = new UserViewModel
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
        };

        return newUserViewModel;
    }

    public IList<UserViewModel> Map(IList<AppUser> users)
    {
        var usersList = new List<UserViewModel>();
        foreach (var user in users)
        {
            usersList.Add(Map(user));
        }

        return usersList;
    }
}