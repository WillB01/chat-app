using ChatApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public interface IUserService
    {
        IEnumerable<AppUser> GetAppUsers { get; }
        Task<IdentityResult> CreateUserAsync(User newUser);
        Task<SignInResult> LoginAsync(User user);
        Task LogoutAsync();
    }
}
