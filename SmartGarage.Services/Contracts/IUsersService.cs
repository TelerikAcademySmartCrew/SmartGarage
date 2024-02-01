using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models;
using System.Security.Claims;

namespace SmartGarage.Services.Services.Contracts
{
    public interface IUsersService
    {
        Task<IdentityResult> Create(AppUser appUser);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<AppUser> GetByEmail(string email);
        Task<bool> UserWithEmailExists(string email);
    }
}
