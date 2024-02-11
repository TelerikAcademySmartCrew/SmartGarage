using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IUsersService
    {
        Task<ICollection<AppUser>> GetAll();
        Task<ICollection<AppUser>> GetUsersInRoleAsync(string role);
        Task<IdentityResult> CreateUser(AppUser appUser);
        Task<IdentityResult> CreateEmployee(AppUser appUser);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<AppUser> GetByEmail(string email);
        Task<AppUser> Update(AppUser appUser);
        Task<bool> Delete(AppUser user);
        Task<bool> UserWithEmailExists(string email);
    }
}
