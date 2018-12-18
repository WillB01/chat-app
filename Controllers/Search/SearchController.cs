using ChatApp.Services;
using ChatApp.Services.FriendRequestService;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Search
{
    public class SearchController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SearchController(IUserService userService, IFriendRequestService friendRequestService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _friendRequestService = friendRequestService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<TypeaheadUsersVM[]> Friends([FromQuery] string q)
        {
            var queryString = q.ToLower();
            var users = await _userService.GetAppUsers();
            var viewModel = users.Where(u => u.UserName.ToLower().StartsWith(queryString))
                .Select(p =>  new TypeaheadUsersVM
            {
                UserName = p.UserName,
                Id = p.Id
            }).ToArray();
            
            return viewModel;
        }

        [HttpPost]
        public async Task PostSearchResult([FromBody] TypeaheadUsersVM person)
        {
            var userName = await Task.Run(() =>  _httpContextAccessor.HttpContext.User.Identity.Name);
            var userId = await _userService.GetUserId(userName);

            var friendRequestVM = new FriendRequestVM
            {
                FromUser = userId,
                ToUser = person.Id,
                HasAccepted = false
            };

             await _friendRequestService.SendFriendRequest(friendRequestVM);

            
        }
    }
}
