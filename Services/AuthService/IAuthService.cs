using ChatApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Services.AuthService
{
    public interface IAuthService
    {
        Task<IdentityResult> CreateUserAsync(MainVM newUser);

        Task<SignInResult> LoginAsync(MainVM user);

        Task LogoutAsync();
    }
}
