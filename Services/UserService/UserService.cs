using ChatApp.Models.Context;
using ChatApp.Models.Identity;
using ChatApp.Services.ViewModelService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Http;
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
        private readonly IViewModelService _viewModelService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext dataContext, UserManager<AppUser> userManager, IViewModelService viewModelService,
             IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _viewModelService = viewModelService;
            _httpContextAccessor = httpContextAccessor;
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

       
        public async Task<IdentityUserVM> GetloggedinUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
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

        public async Task<string> GetUserNameById(string id)
        {
            var result = await Task.Run(() => _dataContext.Users.Where(p => p.Id == id)
            .Select(e => e.UserName).FirstOrDefault());
            return result;
        }
    }
}