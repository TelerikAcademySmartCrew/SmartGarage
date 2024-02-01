using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Services.Contracts
{
    public interface IUsersService
    {
        Task<IdentityResult> Create(AppUser appUser);
        Task<AppUser> GetByEmail(string email);
        Task<bool> UserWithEmailExists(string email);
    }
}
