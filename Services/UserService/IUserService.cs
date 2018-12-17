using ChatApp.Models.Identity;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IUserService
    {
        Task<IdentityUserVM[]> GetAppUsers();

        Task<IdentityResult> CreateUserAsync(MainVM newUser);

        Task<SignInResult> LoginAsync(MainVM user);

        Task LogoutAsync();

        Task<IdentityUserVM> GetloggedinUser(ClaimsPrincipal user);

        Task<IdentityUserVM> GetUserByUserName(string name);

        Task<string> GetUserId(string name);
    }
}