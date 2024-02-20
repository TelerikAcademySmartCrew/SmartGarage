using SmartGarage.Data.Models;
using SmartGarage.Common.Models.RequestDtos;
using SmartGarage.Common.Models.ViewModels;
using SmartGarage.Utilities.Mappers.Contracts;

namespace SmartGarage.Utilities.Mappers
{
    public class UserMapper : IUserMapper
    {
        private readonly IVehicleMapper vehicleMapper;
        private readonly IVisitMapper visitMapper;

        public UserMapper(IVehicleMapper vehicleMapper, IVisitMapper visitMapper)
        {
            this.vehicleMapper = vehicleMapper;
            this.visitMapper = visitMapper;
        }

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
                Vehicles = vehicleMapper.ToViewModel(user.Vehicles),
                Visits = visitMapper.ToViewModel(user.Visits),
            };

            return newUserViewModel;
        }

        public ICollection<UserViewModel> Map(ICollection<AppUser> users)
        {
            var usersList = new List<UserViewModel>();
            foreach (var user in users)
            {
                usersList.Add(Map(user));
            }

            return usersList;
        }
    }
}