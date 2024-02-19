using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;

namespace SmartGarage.Utilities.Mappers.Contracts;

public interface IUserMapper
{
    AppUser MaterializeRequestDto(UserRegisterRequestDto userRegisterRequestDto);

    ICollection<UserViewModel> Map(ICollection<AppUser> users);

    UserViewModel Map(AppUser user);
}