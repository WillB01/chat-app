﻿using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAppUsers { get; }

        Task<IdentityResult> CreateUserAsync(MainViewModel newUser);

        Task<SignInResult> LoginAsync(MainViewModel user);

        Task LogoutAsync();

        Task<IdentityUserVM> GetloggedinUser(ClaimsPrincipal user);

        Task<AppUser> GetUserByUserName(string name);

        Task<string> GetUserId(string name);
    }
}