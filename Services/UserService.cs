using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Models.Context;
using ChatApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(DataContext dataContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IEnumerable<AppUser> GetAppUsers => _dataContext.Users;

        

        public async Task<IdentityResult> CreateUserAsync(User newUser)
        {
            var appUser = new AppUser
            {
                UserName = newUser.Name,
                Email = newUser.Email,
            };
            
            var result = await _userManager.CreateAsync(appUser, newUser.Password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(User user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Name, user.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
