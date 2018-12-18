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

        public async Task<IdentityUserVM[]> GetAppUsers()
        {
            var result = await Task.Run(() =>_dataContext.Users
            .Select(e => new IdentityUserVM
            {
                Id = e.Id,
                UserName = e.UserName,
                Email = e.Email,
            })
            .ToArray());

            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(MainVM user)
        {
            var appUser = new AppUser
            {
                UserName = user.RegisterNewVM.Name,
                Email = user.RegisterNewVM.Email,
            };

            var result = await _userManager.CreateAsync(appUser, user.RegisterNewVM.Password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(MainVM user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserLoginVM.Name, user.UserLoginVM.Password, false, false);
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityUserVM> GetloggedinUser(ClaimsPrincipal user)
        {
            var result = await _userManager.GetUserAsync(user);
            var viewModel = new IdentityUserVM
            {
                Id = result.Id,
                UserName = result.UserName,
                Email = result.Email
            };
            return viewModel;
        }

        public async Task<IdentityUserVM> GetUserByUserName(string name)
        {
            var result = await Task.Run(() => _dataContext.Users.Where(p => p.UserName == name)
                .FirstOrDefault());
            var viewModel = new IdentityUserVM
            {
                Id = result.Id,
                UserName = result.UserName,
                Email = result.Email
            };
            

            return viewModel;
        }

        public async Task<string> GetUserId(string name)
        {
            var result = await Task.Run(() => _dataContext.Users.Where(p => p.UserName == name)
                .FirstOrDefault().Id);
            return result;
        }
    }
}