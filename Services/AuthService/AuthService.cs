using ChatApp.Models.Identity;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ChatApp.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IViewModelService _viewModelService;

        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IViewModelService viewModelService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _viewModelService = viewModelService;
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
    }
}