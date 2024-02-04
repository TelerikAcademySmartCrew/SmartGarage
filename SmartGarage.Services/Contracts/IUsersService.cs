﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SmartGarage.Data.Models;

namespace SmartGarage.Services.Contracts
{
    public interface IUsersService
    {
        Task<IdentityResult> Create(AppUser appUser);
        Task<AppUser> GetUserAsync(ClaimsPrincipal user);
        Task<AppUser> GetByEmail(string email);
        Task<bool> UserWithEmailExists(string email);
    }
}
