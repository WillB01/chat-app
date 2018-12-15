using ChatApp.Models.Context;
using ChatApp.Models.Identity;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IViewModelService _viewModelService;

        public UserService(DataContext dataContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IViewModelService viewModelService)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _viewModelService = viewModelService;
        }

        public IEnumerable<AppUser> GetAppUsers => _dataContext.Users;

        public async Task<IdentityResult> CreateUserAsync(MainViewModel user)
        {
            var appUser = new AppUser
            {
                UserName = user.RegisterNewUserView.Name,
                Email = user.RegisterNewUserView.Email,
            };

            var result = await _userManager.CreateAsync(appUser, user.RegisterNewUserView.Password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(MainViewModel user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserLoginViewModel.Name, user.UserLoginViewModel.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AppUser> GetloggedinUser(ClaimsPrincipal user)
        {
            var result = await _userManager.GetUserAsync(user);
            return result;
        }

        public async Task<AppUser> GetUserByUserName(string name)
        {
            var result = await Task.Run(() => _dataContext.Users.Where(p => p.UserName == name)
                .FirstOrDefault());
            return result;
        }

        public async Task<string> GetUserId(string name)
        {
            var result = await Task.Run(() => _dataContext.Users.Where(p => p.UserName == name)
                .FirstOrDefault().Id);
            return result;
        }
    }
}